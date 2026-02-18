using GarageManagement.Server.dtos;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IUserRepository
    {
        Task<List<UserListDto>> GetUsersAsync();
        Task<BaseResultDto> UpdateUserRoleAsync(long userId, long roleId);
    }
}
