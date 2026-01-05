using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IRoleRepository
    {
        Task<List<RoleModel>> GetAllRolesAsync();
        Task<RoleModel> GetRoleByIdAsync(long id);
        Task<BaseResultDto> AddRoleAsync(RoleModel role);
        Task<BaseResultDto> DeleteRoleAsync(long id);
    }
}
