using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IRoleRepository
    {
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(long id);
        Task<BaseResultDto> AddRoleAsync(RoleDto role);
        Task<BaseResultDto> DeleteRoleAsync(long id);
    }
}
