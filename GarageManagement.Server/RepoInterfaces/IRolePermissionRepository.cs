using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IRolePermissionRepository
    {
        Task<List<RolePermissionDto>> GetRolePermissionsAsync(long roleId);
        Task<List<string>> GetRoleModulePermissionsAsync(long roleId, string moduleName);
        Task<BaseResultDto> SaveRolePermissionAsync(RolePermissionDto dto);
        Task<BaseResultDto> RemoveRolePermissionAsync(long roleId, long permissionId, string moduleName);
        Task<BaseResultDto> ClearRoleModulePermissionsAsync(long roleId, string moduleName);
        Task<bool> HasPermissionAsync(long roleId, string moduleName, string permissionName);
    }
}
