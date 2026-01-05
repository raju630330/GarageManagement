using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IRolePermissionRepository
    {
        // Role-Permission management
        Task<List<RolePermissionDto>> GetRolePermissionsAsync(long roleId);
        Task<List<string>> GetRoleModulePermissionsAsync(long roleId, string moduleName);
        Task<BaseResultDto> SaveRolePermissionAsync(RolePermissionDto dto);
        Task<BaseResultDto> RemoveRolePermissionAsync(long roleId, long permissionId, long moduleId);
        Task<BaseResultDto> ClearRoleModulePermissionsAsync(long roleId, long moduleId);
        Task<bool> HasPermissionAsync(long roleId, string moduleName, string permissionName);

        // Module management
        Task<BaseResultDto> AddModuleAsync(string moduleName, string description = null);
        Task<List<PermissionModule>> GetAllModulesAsync();
    }
}
