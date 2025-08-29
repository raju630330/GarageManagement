using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class User
    {
        [Key]  // Primary Key
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")] public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]

        public string PasswordHash { get; set; }
        public string Role { get; set; } = "Driver";
        public string? ResetTokenHash { get; set; }
        public DateTime? ResetTokenExpiresUtc { get; set; }

        // Navigation: One User → Many Appointments
        public ICollection<BookAppointment> BookAppointments { get; set; }
    }

}
