using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IRolePermissionRepository
    {
        /* ===================== ROLES ===================== */
        Task<List<RoleDto>> GetAllRolesAsync();

        /* ===================== MODULES ===================== */
        Task<List<PermissionModule>> GetAllModulesAsync();
        Task<BaseResultDto> AddModuleAsync(string moduleName);

        /* ===================== ROLE ↔ MODULE ↔ PERMISSION ===================== */
        Task<List<RolePermissionDto>> GetRolePermissionsAsync(long roleId); // returns Role + Module + permission(s)
        Task<BaseResultDto> SaveRolePermissionsAsync(
    List<RolePermissionDto> permissions);

    }
}
