using GarageManagement.Server.Model;

namespace GarageManagement.Server.dtos
{
    public class RolePermissionDto
    {
        public long Id { get; set; }
        public string ModuleName { get; set; }
        public long RoleId { get; set; }
        public long? PermissionId { get; set; }
        public long? PermissionModuleId { get; set; }
    }
}
