using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]

        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; } = default!;

        public string Role { get; set; } = "Driver"; // Admin | Driver | Technician
    }

    public class LoginDto
    {
        public string UsernameOrEmail { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = default!;
        public String message { get; set; }
    }

    public sealed class ForgotPasswordDto
    {
        public string EmailOrUsername { get; set; } = default!;
    }

    public sealed class ResetPasswordDto
    {
        public string EmailOrUsername { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        //public string NewPassword { get; set; } = default!;
    }

    public class ProfileView
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
