using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class Permission
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } // e.g., "ViewAppointments", "EditJobCards"

        public string Description { get; set; }

        // Navigation
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
