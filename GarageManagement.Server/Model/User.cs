using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? ResetTokenHash { get; set; }
        public DateTime? ResetTokenExpiresUtc { get; set; }
        public long RoleId { get; set; }
        public bool? IsActive { get; set; }
        public Role Role { get; set; }
        public ICollection<WorkshopUser> WorkshopUsers { get; set; } 
        public ICollection<BookAppointment> BookAppointments { get; set; }
        public ICollection<StockMovement> StockMovements { get; set; }
    }
}
