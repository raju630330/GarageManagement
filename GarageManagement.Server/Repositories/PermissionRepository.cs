using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;
        public PermissionRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
        {
            return await _context.Permissions
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                })
                .ToListAsync();
        }

        public async Task<PermissionDto> GetPermissionByIdAsync(long id)
        {
            return await _context.Permissions
                .Where(p => p.Id == id)
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                })
                .FirstOrDefaultAsync();
        }

        // ✅ ADD + UPDATE (SINGLE METHOD)
        public async Task<BaseResultDto> SavePermissionAsync(PermissionDto dto)
        {
            // 🔴 DUPLICATE CHECK
            bool exists = await _context.Permissions
                .AnyAsync(p =>
                    p.Name.ToLower() == dto.Name.ToLower()
                    && p.Id != dto.Id);

            if (exists)
            {
                return new BaseResultDto
                {
                    IsSuccess = false,
                    Message = "Permission already exists"
                };
            }

            Permission entity = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (entity == null)
                entity = new Permission();

            entity.Name = dto.Name;
            entity.Description = dto.Description;

            if (dto.Id == 0)
                await _context.Permissions.AddAsync(entity);
            else
                _context.Permissions.Update(entity);

            await _context.SaveChangesAsync();

            return new BaseResultDto
            {
                Id = entity.Id,
                IsSuccess = true,
                Message = dto.Id == 0
                    ? "Permission added successfully"
                    : "Permission updated successfully"
            };
        }

        // ✅ SAFE DELETE
        public async Task<BaseResultDto> DeletePermissionAsync(long id)
        {
            bool isAssigned = await _context.RolePermissions
                .AnyAsync(rp => rp.PermissionId == id);

            if (isAssigned)
            {
                return new BaseResultDto
                {
                    Id = id,
                    IsSuccess = false,
                    Message = "Permission is assigned to roles and cannot be deleted"
                };
            }

            var perm = await _context.Permissions.FindAsync(id);
            if (perm == null)
            {
                return new BaseResultDto
                {
                    Id = id,
                    IsSuccess = false,
                    Message = "Permission not found"
                };
            }

            _context.Permissions.Remove(perm);
            await _context.SaveChangesAsync();

            return new BaseResultDto
            {
                Id = id,
                IsSuccess = true,
                Message = "Permission deleted successfully"
            };
        }
    }
}
