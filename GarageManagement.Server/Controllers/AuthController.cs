using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace GarageManagement.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;
        private readonly EmailService _emailService;


        public AuthController(ApplicationDbContext db, IConfiguration config,EmailService emailService)
        {
            _db = db;
            _config = config;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = dto.Username.Trim();
            var email = dto.Email.Trim().ToLower();

            // Check username OR email existence in ONE query
            var existingUser = await _db.Users
                .Where(u => u.Username == username || u.Email == email)
                .Select(u => new { u.RoleId })
                .FirstOrDefaultAsync();

            if (existingUser != null)
                return BadRequest("Username or Email already exists.");

            // Only one Admin allowed
            if (dto.RoleId == 1)
            {
                bool adminExists = await _db.Users.AnyAsync(u => u.RoleId == 1);
                if (adminExists)
                    return BadRequest("Only one Admin is allowed.");
            }

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = dto.RoleId,
                IsActive = true
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                message = "Registered successfully. Please login."
            });
        }


        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UsernameOrEmail) ||
                string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Username and password are required.");

            var user = await _db.Users
                .Include(u => u.Role)
                .Include(u => u.WorkshopUsers.Where(w => w.IsActive))
                .FirstOrDefaultAsync(u =>
                    u.Username == dto.UsernameOrEmail ||
                    u.Email == dto.UsernameOrEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            long? workshopId = user.WorkshopUsers?
                .Select(w => w.WorkshopId)
                .FirstOrDefault();

            // 🚀 Bootstrap logic
            if (workshopId == null || workshopId == 0)
            {
                if (user.Role.RoleName != "Admin")
                    return Unauthorized("User is not assigned to any workshop.");

                workshopId = 0; // temporary bootstrap mode
            }

            var token = GenerateJwt(user, workshopId);

            return Ok(new AuthResponse
            {
                Token = token,
                message = "Login successful"
            });
        }
        [Authorize]
        [HttpGet("me")]
        public ActionResult<ProfileView> me()
        {
            var username = User.Identity?.Name ?? string.Empty;
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                              ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(sub))
                return Unauthorized("Invalid token – missing subject claim.");

            if (!int.TryParse(sub, out var userId))
                return Unauthorized("Invalid token – subject is not a valid user id.");

            return Ok(new ProfileView
            {
                Id = userId,
                Username = username,
                Email = email,
                Role = role
            });
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.EmailOrUsername))
                return BadRequest("Email is required.");

            var email = dto.EmailOrUsername.Trim().ToLower();

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

            if (user == null)
                return Ok(new { message = "If account exists, reset link sent." }); // prevent email enumeration

            // Generate secure random token
            var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var tokenHash = Sha256(rawToken);

            user.ResetTokenHash = tokenHash;
            user.ResetTokenExpiresUtc = DateTime.UtcNow.AddMinutes(30);

            await _db.SaveChangesAsync();

            var frontendUrl = "http://localhost:4200/reset-password";
            var callbackUrl = $"{frontendUrl}?token={WebUtility.UrlEncode(rawToken)}&email={user.Email}";

            var mailRequest = new EmailRequest
            {
                ToEmail = user.Email,
                Subject = "Password Reset Request",
                Body = $@"
            <p>Click the link below to reset your password:</p>
            <a href='{callbackUrl}'>Reset Password</a>
            <p>This link expires in 30 minutes.</p>"
            };

            await _emailService.SendEmailAsync(mailRequest);

            return Ok(new { message = "If account exists, reset link sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Token) ||
                string.IsNullOrWhiteSpace(request.EmailOrUsername) ||
                string.IsNullOrWhiteSpace(request.NewPassword))
                return BadRequest("Invalid request.");

            var email = request.EmailOrUsername.Trim().ToLower();
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);

            if (user == null)
                return BadRequest("Invalid token.");

            var incomingTokenHash = Sha256(request.Token);

            if (user.ResetTokenHash != incomingTokenHash ||
                user.ResetTokenExpiresUtc == null ||
                user.ResetTokenExpiresUtc < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired token.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.ResetTokenHash = null;
            user.ResetTokenExpiresUtc = null;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Password reset successfully." });
        }


        // Helpers
        private string GenerateJwt(User user, long? workshopId)
        {
            var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Name, user.Username),
        new(ClaimTypes.Email, user.Email),
        new(ClaimTypes.Role, user.Role!.RoleName),
        new("roleId", user.Role.Id.ToString()),
        new("workshopId", workshopId?.ToString() ?? "0"),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["Jwt:ExpiresInMinutes"]!)
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminData() => Ok("This is admin-only data.");

        [HttpGet("driver")]
        [Authorize(Roles = "Driver")]
        public IActionResult DriverData() => Ok("This is driver-only data.");

        [HttpGet("technician")]
        [Authorize(Roles = "Technician")]
        public IActionResult TechnicianData() => Ok("This is technician-only data.");

        [HttpGet("Cashier")]
        [Authorize(Roles = "Cashier")]
        public IActionResult CashierData() => Ok("This is Cashier-only data.");

        [HttpGet("Manager")]
        [Authorize(Roles = "Manager")]
        public IActionResult ManagerData() => Ok("This is Manager-only data.");

        [HttpGet("Supervisor")]
        [Authorize(Roles = "Supervisor")]
        public IActionResult SupervisorData() => Ok("This is Supervisor-only data.");

        private static string Sha256(string value)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToHexString(bytes);
        }

        [HttpGet("GetRoles")]
        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            return await _db.Roles
                .Select(r => new RoleDto { Id = r.Id, RoleName = r.RoleName })
                .ToListAsync();
        }



    }
}

    

