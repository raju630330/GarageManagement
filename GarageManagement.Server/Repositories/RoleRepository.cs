using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<RoleDto>> GetAllRolesAsync() =>
            await _context.Roles.Select(a => new
             RoleDto()
            {
                Id = a.Id,
                RoleName = a.RoleName,
            }).ToListAsync();

        public async Task<RoleDto> GetRoleByIdAsync(long id) =>
            await _context.Roles.Where(a => a.Id == id).Select(a => new RoleDto(){
                Id = a.Id,
                RoleName = a.RoleName
            }).FirstOrDefaultAsync();

        public async Task<BaseResultDto> AddRoleAsync(RoleDto role)
        {
            // 🔴 DUPLICATE ROLE CHECK (CASE INSENSITIVE)
            var exists = await _context.Roles
                .AnyAsync(r =>
                    r.RoleName.ToLower() == role.RoleName.ToLower()
                    && r.Id != role.Id);

            if (exists)
            {
                return new BaseResultDto
                {
                    IsSuccess = false,
                    Message = "Role already exists"
                };
            }

            var entity = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == role.Id);

            if (entity == null)
                entity = new Role();

            entity.RoleName = role.RoleName;

            if (role.Id == 0)
                await _context.Roles.AddAsync(entity);
            else
                _context.Roles.Update(entity);

            await _context.SaveChangesAsync();

            return new BaseResultDto
            {
                Id = entity.Id,
                IsSuccess = true,
                Message = role.Id == 0 ? "Role added successfully" : "Role updated successfully"
            };
        }


        public async Task<BaseResultDto> DeleteRoleAsync(long id)
        {
            // 🔴 CHECK IF ROLE ASSIGNED TO ANY USER
            bool isAssigned = await _context.Users
                .AnyAsync(u => u.RoleId == id);

            if (isAssigned)
            {
                return new BaseResultDto
                {
                    Id = id,
                    IsSuccess = false,
                    Message = "Role is assigned to users and cannot be deleted"
                };
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return new BaseResultDto
                {
                    Id = id,
                    IsSuccess = false,
                    Message = "Role not found"
                };
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return new BaseResultDto
            {
                Id = id,
                IsSuccess = true,
                Message = "Role deleted successfully"
            };
        }
    }
}
