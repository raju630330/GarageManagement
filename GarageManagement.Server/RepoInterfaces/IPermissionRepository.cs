using GarageManagement.Server.Model;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAllPermissionsAsync();
        Task<Permission> GetPermissionByIdAsync(long id);
        Task AddPermissionAsync(Permission permission);
        Task UpdatePermissionAsync(Permission permission);
        Task DeletePermissionAsync(long id);
    }
}
