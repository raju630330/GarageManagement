using GarageManagement.Server.Model;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IRolePermissionRepository
    {
        Task<List<RolePermission>> GetRolePermissionsAsync(long roleId);
        Task<List<string>> GetRoleModulePermissionsAsync(long roleId, string moduleName);
        Task AddRolePermissionAsync(RolePermission rolePermission);
        Task RemoveRolePermissionAsync(long roleId, long permissionId, string moduleName);
        Task<bool> HasPermissionAsync(long roleId, string moduleName, string permissionName);
    }
}
