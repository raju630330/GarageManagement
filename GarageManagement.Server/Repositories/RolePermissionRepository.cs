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
        public RolePermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /* ===================== ROLES ===================== */
        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Select(r => new RoleDto { Id = r.Id, RoleName = r.RoleName })
                .ToListAsync();
        }

        /* ===================== MODULES ===================== */
        public async Task<List<PermissionModule>> GetAllModulesAsync()
        {
            return await _context.PermissionModules
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<BaseResultDto> AddModuleAsync(string moduleName)
        {
            if (string.IsNullOrWhiteSpace(moduleName))
                return new BaseResultDto { IsSuccess = false, Message = "Module name is required" };

            bool exists = await _context.PermissionModules.AnyAsync(m => m.Name == moduleName);
            if (exists)
                return new BaseResultDto { IsSuccess = false, Message = "Module already exists" };

            var module = new PermissionModule { Name = moduleName };
            _context.PermissionModules.Add(module);
            await _context.SaveChangesAsync();

            return new BaseResultDto { IsSuccess = true, Message = "Module added successfully" };
        }

        /* ===================== ROLE ↔ MODULE ↔ PERMISSION ===================== */
        public async Task<List<RolePermissionDto>> GetRolePermissionsAsync(long roleId)
        {
            return await _context.RolePermissions
                .Include(rp => rp.PermissionModule)
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => new RolePermissionDto
                {
                    Id = rp.Id,
                    RoleId = rp.RoleId,
                    PermissionModuleId = rp.PermissionModuleId,
                    ModuleName = rp.PermissionModule.Name,
                    PermissionId = rp.PermissionId
                })
                .ToListAsync();
        }

        public async Task<BaseResultDto> SaveRolePermissionsAsync(List<RolePermissionDto> permissions)
        {
            // Delete ALL existing permissions
            var allExisting = await _context.RolePermissions.ToListAsync();
            _context.RolePermissions.RemoveRange(allExisting);

            // Add new permissions if any
            if (permissions != null && permissions.Any())
            {
                var newPerms = permissions.Select(p => new RolePermission
                {
                    RoleId = p.RoleId,
                    PermissionModuleId = p.PermissionModuleId,
                    PermissionId = p.PermissionId
                });

                await _context.RolePermissions.AddRangeAsync(newPerms);
            }

            await _context.SaveChangesAsync();

            return new BaseResultDto
            {
                IsSuccess = true,
                Message = "Permissions saved successfully"
            };
        }
    }
}
