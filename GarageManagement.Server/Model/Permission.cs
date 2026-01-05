using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class Permission
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
