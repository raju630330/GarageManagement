using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly ApplicationDbContext _context;
        public RolePermissionRepository(ApplicationDbContext context) => _context = context;

        // Get all permissions for a role
        public async Task<List<RolePermissionDto>> GetRolePermissionsAsync(long roleId)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => new RolePermissionDto
                {
                    Id = rp.Id,
                    RoleId = rp.RoleId,
                    PermissionId = rp.PermissionId,
                    ModuleName = rp.ModuleName
                })
                .ToListAsync();
        }

        // Get permission names by role and module
        public async Task<List<string>> GetRoleModulePermissionsAsync(long roleId, string moduleName)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => rp.RoleId == roleId && rp.ModuleName == moduleName)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();
        }

        // ✅ Add or update single role-permission
        public async Task<BaseResultDto> SaveRolePermissionAsync(RolePermissionDto dto)
        {
            // Check for duplicates
            bool exists = await _context.RolePermissions.AnyAsync(rp =>
                rp.RoleId == dto.RoleId &&
                rp.ModuleName == dto.ModuleName &&
                rp.PermissionId == dto.PermissionId &&
                rp.Id != dto.Id);

            if (exists)
            {
                return new BaseResultDto
                {
                    IsSuccess = false,
                    Message = "This permission is already assigned to the role for this module"
                };
            }

            RolePermission entity = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.Id == dto.Id);

            if (entity == null)
                entity = new RolePermission();

            entity.RoleId = dto.RoleId;
            entity.PermissionId = dto.PermissionId;
            entity.ModuleName = dto.ModuleName;

            if (dto.Id == 0)
                await _context.RolePermissions.AddAsync(entity);
            else
                _context.RolePermissions.Update(entity);

            await _context.SaveChangesAsync();

            return new BaseResultDto
            {
                Id = entity.Id,
                IsSuccess = true,
                Message = dto.Id == 0 ? "Permission assigned successfully" : "Permission updated successfully"
            };
        }

        // ✅ Remove single permission
        public async Task<BaseResultDto> RemoveRolePermissionAsync(long roleId, long permissionId, string moduleName)
        {
            var rp = await _context.RolePermissions
                .FirstOrDefaultAsync(x =>
                    x.RoleId == roleId &&
                    x.PermissionId == permissionId &&
                    x.ModuleName == moduleName);

            if (rp == null)
            {
                return new BaseResultDto
                {
                    IsSuccess = false,
                    Message = "Permission not found for this role and module"
                };
            }

            _context.RolePermissions.Remove(rp);
            await _context.SaveChangesAsync();

            return new BaseResultDto
            {
                IsSuccess = true,
                Message = "Permission removed successfully"
            };
        }

        // ✅ Clear all permissions of a module for a role
        public async Task<BaseResultDto> ClearRoleModulePermissionsAsync(long roleId, string moduleName)
        {
            var items = _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && rp.ModuleName == moduleName);

            _context.RolePermissions.RemoveRange(items);
            await _context.SaveChangesAsync();

            return new BaseResultDto
            {
                IsSuccess = true,
                Message = "All permissions cleared for this module"
            };
        }

        // Check if role has a specific permission
        public async Task<bool> HasPermissionAsync(long roleId, string moduleName, string permissionName)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Permission)
                .AnyAsync(rp =>
                    rp.RoleId == roleId &&
                    rp.ModuleName == moduleName &&
                    rp.Permission.Name == permissionName);
        }
    }
}
