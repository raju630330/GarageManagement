using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        { 
            _context = context; 
        }
        public async Task<List<UserListDto>> GetUsersAsync()
        {
            var users = await _context.Users.Include(a=> a.Role).Where(a=> a.RoleId != 1).ToListAsync();

            return users.Select(u => new UserListDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                RoleName = u.Role?.RoleName ?? "N/A",
                RoleId = u.RoleId,  
                IsActive = u.IsActive  // map the active status
            }).ToList();
        }
        public async Task<BaseResultDto> UpdateUserRoleAsync(long userId, long roleId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new BaseResultDto() {IsSuccess = false,Message = "User Not Exist" };

            user.RoleId = roleId;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new BaseResultDto() { IsSuccess = true, Message = "Role changed successfully" };
        }
    }
}
