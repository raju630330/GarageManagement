using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; } = string.Empty;
        public ICollection<User>? Users { get; set; }
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
