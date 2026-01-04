using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class RolePermission
    {
        public long Id { get; set; }
        public string ModuleName { get; set; }
        public long RoleId { get; set; }
        public Role Role { get; set; }

        public long PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
