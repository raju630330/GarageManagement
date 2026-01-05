using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IPermissionRepository
    {
        Task<List<PermissionDto>> GetAllPermissionsAsync();
        Task<PermissionDto> GetPermissionByIdAsync(long id);
        Task<BaseResultDto> SavePermissionAsync(PermissionDto permission);
        Task<BaseResultDto> DeletePermissionAsync(long id);
    }
}
