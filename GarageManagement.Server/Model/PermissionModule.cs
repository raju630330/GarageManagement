using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class PermissionModule : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
