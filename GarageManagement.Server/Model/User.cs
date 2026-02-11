using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class User
    {
        public long Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public string? ResetTokenHash { get; set; }
        public DateTime? ResetTokenExpiresUtc { get; set; }

        // FK to Role
        public long RoleId { get; set; }
        public Role Role { get; set; }

        // Navigation
        public ICollection<WorkshopUser> WorkshopUsers { get; set; }
        public ICollection<BookAppointment> BookAppointments { get; set; }
        public ICollection<StockMovement> StockMovements { get; set; }
    }
}
