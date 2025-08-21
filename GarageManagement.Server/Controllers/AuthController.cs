using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
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

        public AuthController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // Register: always creates a normal User (no admin creation from API)
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email))
                return BadRequest("Username or Email already exists");

            var user = new User
            {
                Username = dto.Username.Trim(),
                Email = dto.Email.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Auto-login after register
            var token = GenerateJwt(user);
            return Ok(new AuthResponse { Token = token });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginDto dto)
        {
            var user = await _db.Users
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

            if(!int.TryParse(sub, out var userId))
            return Unauthorized("Invalid token – subject is not a valid user id.");


            return Ok(new ProfileView
            {
                Id = userId,
                Username = username,
                Email = email,
                Role = role
            });
        }

        

        // Forgot password: generate short-lived token (store only the hash)
        [HttpPost("forgot")]
        public async Task<ActionResult<object>> Forgot(ForgotPasswordDto dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == dto.EmailOrUsername || u.Username == dto.EmailOrUsername);

            if (user == null) return Ok(new { message = "If the account exists, a reset link has been sent." });

            var rawToken = Guid.NewGuid().ToString("N");                 
            user.ResetTokenHash = Sha256(rawToken);                       
            user.ResetTokenExpiresUtc = DateTime.UtcNow.AddMinutes(15);   
            await _db.SaveChangesAsync();

            return Ok(new { message = "Reset token generated", devToken = rawToken });
        }

        [HttpPost("reset")]
        public async Task<ActionResult<object>> Reset(ResetPasswordDto dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == dto.EmailOrUsername || u.Username == dto.EmailOrUsername);

            if (user == null || user.ResetTokenHash == null || user.ResetTokenExpiresUtc == null)
                return BadRequest("Invalid or expired token");

            if (user.ResetTokenExpiresUtc < DateTime.UtcNow)
                return BadRequest("Invalid or expired token");

            if (Sha256(dto.Token) != user.ResetTokenHash)
                return BadRequest("Invalid or expired token");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.ResetTokenHash = null;
            user.ResetTokenExpiresUtc = null;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Password reset successful" });
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
            new(ClaimTypes.Role, user.Role),
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

        private static string Sha256(string value)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToHexString(bytes);
        }

        
    }
}
