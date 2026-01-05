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
            var validRoles = new[] { "Admin", "Driver", "Technician", "Cashier", "Manager", "Supervisor" };

            if (string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Username, Email and Password cannot be empty.");
            }

            dto.Role = char.ToUpper(dto.Role[0]) + dto.Role.Substring(1).ToLower();

            if (!validRoles.Contains(dto.Role))
            {
                return BadRequest("Invalid role. Allowed: Admin, Driver, Technician.");
            }

            if (dto.Role == "Admin" && await _db.Users.AnyAsync(u => u.Role.RoleName == "Admin"))
            {
                return BadRequest("Only one Admin is allowed.");
            }

            if (await _db.Users.AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email))
            {
                return BadRequest("Username or Email already exists.");
            }

            var user = new User
            {
                Username = dto.Username.Trim(),
                Email = dto.Email.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = new Role { RoleName = dto.Role}
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            //var token = GenerateJwt(user);
            //return Ok(new AuthResponse { Token = token, message = "Registered successfully. Please login." });
            return Ok(new AuthResponse { message = "Registered successfully. Please login." });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody]  LoginDto dto)
        {
            var user = await _db.Users.Include(a=>a.Role)
                .FirstOrDefaultAsync(u => u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = GenerateJwt(user);
            return Ok(new AuthResponse { Token = token });
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
            if (string.IsNullOrEmpty(dto.EmailOrUsername))
                return BadRequest("Email is required");

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.EmailOrUsername.Trim().ToLower());

            if (user == null) return BadRequest("User not found");


            // Generate JWT reset token
            var token = GenerateJwt(user);
            var frontendUrl = "http://localhost:4200/reset-password";
            var callbackUrl = $"{frontendUrl}?token={token}&email={user.Email}";

            var mailRequest = new EmailRequest
            {
                ToEmail = user.Email,
                Subject = "Password Reset Request",
                Body = $"<p>Click the link below to reset your password:</p><a href='{callbackUrl}'>Reset Password</a>"
            };

            await _emailService.SendEmailAsync(mailRequest);

            return Ok(new { message = "Reset link has been sent to your email" });

        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(request.Token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                    ValidateLifetime = true
                }, out var validatedToken);

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _db.Users.FindAsync(int.Parse(userId));

                if (user == null)
                    return BadRequest("Invalid token");

                // Update password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                user.ResetTokenHash = null;
                user.ResetTokenExpiresUtc = null;
                await _db.SaveChangesAsync();

                return Ok(new { message = "Password has been reset successfully." });
            }
            catch
            {
                return BadRequest("Invalid or expired token.");
            }

        }
        


        // Helpers
        private string GenerateJwt(User user)
        {
            var claims = new List<Claim>
        {
            // include both to be safe
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.RoleName),
            new Claim("roleId", user.Role.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiresInMinutes"]!)),
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


       
    }
}

    

