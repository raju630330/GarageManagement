using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class Role
    {
        public long Id { get; set; }

        [Required]
        public string RoleName { get; set; } // e.g., Service Advisor, Technician, Cashier

        // Navigation
        public ICollection<User> Users { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
