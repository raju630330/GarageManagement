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

        // ===========================
        // MODULE MANAGEMENT
        // ===========================

        // Add a new module dynamically
        public async Task<BaseResultDto> AddModuleAsync(string moduleName, string description = null)
        {
            if (string.IsNullOrWhiteSpace(moduleName))
                return new BaseResultDto { IsSuccess = false, Message = "Module name is required" };

            bool exists = await _context.PermissionModules.AnyAsync(m => m.Name == moduleName);
            if (exists)
                return new BaseResultDto { IsSuccess = false, Message = "Module already exists" };

            var module = new PermissionModule
            {
                Name = moduleName,
                Description = description
            };

            await _context.PermissionModules.AddAsync(module);
            await _context.SaveChangesAsync();

            return new BaseResultDto
            {
                IsSuccess = true,
                Id = module.Id,
                Message = "Module added successfully"
            };
        }

        // Get all modules
        public async Task<List<PermissionModule>> GetAllModulesAsync()
        {
            return await _context.PermissionModules
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        // ===========================
        // ROLE PERMISSION MANAGEMENT
        // ===========================

        // Get all role permissions
        public async Task<List<RolePermissionDto>> GetRolePermissionsAsync(long roleId)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Include(rp => rp.PermissionModule)
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => new RolePermissionDto
                {
                    Id = rp.Id,
                    RoleId = rp.RoleId,
                    PermissionId = rp.PermissionId,
                    ModuleName = rp.PermissionModule.Name,
                    PermissionModuleId = rp.PermissionModuleId
                })
                .ToListAsync();
        }

        // Get permission names for a role + module
        public async Task<List<string>> GetRoleModulePermissionsAsync(long roleId, string moduleName)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Include(rp => rp.PermissionModule)
                .Where(rp => rp.RoleId == roleId && rp.PermissionModule.Name == moduleName)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();
        }

        // Check if a role has a specific permission
        public async Task<bool> HasPermissionAsync(long roleId, string moduleName, string permissionName)
        {
            return await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Include(rp => rp.PermissionModule)
                .AnyAsync(rp =>
                    rp.RoleId == roleId &&
                    rp.PermissionModule.Name == moduleName &&
                    rp.Permission.Name == permissionName);
        }

        // Add or update a role permission
        public async Task<BaseResultDto> SaveRolePermissionAsync(RolePermissionDto dto)
        {
            if (dto == null)
                return new BaseResultDto { IsSuccess = false, Message = "Invalid data" };

            // Check duplicates
            bool exists = await _context.RolePermissions.AnyAsync(rp =>
                rp.RoleId == dto.RoleId &&
                rp.PermissionModuleId == dto.PermissionModuleId &&
                rp.PermissionId == dto.PermissionId &&
                rp.Id != dto.Id);

            if (exists)
                return new BaseResultDto
                {
                    IsSuccess = false,
                    Message = "This permission is already assigned to the role for this module"
                };

            RolePermission entity = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.Id == dto.Id);

            if (entity == null)
            {
                entity = new RolePermission
                {
                    RoleId = dto.RoleId,
                    PermissionModuleId = dto.PermissionModuleId,
                    PermissionId = dto.PermissionId
                };
                await _context.RolePermissions.AddAsync(entity);
            }
            else
            {
                entity.RoleId = dto.RoleId;
                entity.PermissionModuleId = dto.PermissionModuleId;
                entity.PermissionId = dto.PermissionId;
                _context.RolePermissions.Update(entity);
            }

            await _context.SaveChangesAsync();

            return new BaseResultDto
            {
                Id = entity.Id,
                IsSuccess = true,
                Message = dto.Id == 0 ? "Permission assigned successfully" : "Permission updated successfully"
            };
        }

        // Remove a single role permission
        public async Task<BaseResultDto> RemoveRolePermissionAsync(long roleId, long permissionId, long moduleId)
        {
            var rp = await _context.RolePermissions
                .FirstOrDefaultAsync(r =>
                    r.RoleId == roleId &&
                    r.PermissionId == permissionId &&
                    r.PermissionModuleId == moduleId);

            if (rp == null)
                return new BaseResultDto { IsSuccess = false, Message = "Permission not found" };

            _context.RolePermissions.Remove(rp);
            await _context.SaveChangesAsync();

            return new BaseResultDto { IsSuccess = true, Message = "Permission removed successfully" };
        }

        // Clear all permissions for a role + module
        public async Task<BaseResultDto> ClearRoleModulePermissionsAsync(long roleId, long moduleId)
        {
            var list = _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && rp.PermissionModuleId == moduleId);

            _context.RolePermissions.RemoveRange(list);
            await _context.SaveChangesAsync();

            return new BaseResultDto { IsSuccess = true, Message = "All permissions cleared for this module" };
        }
    }
}
