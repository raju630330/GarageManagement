using GarageManagement.Server.Data;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly ApplicationDbContext _context;
        public RolePermissionRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<RolePermission>> GetRolePermissionsAsync(long roleId) =>
            await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();

        public async Task<List<string>> GetRoleModulePermissionsAsync(long roleId, string moduleName) =>
            await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => rp.RoleId == roleId && rp.ModuleName == moduleName)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();

        public async Task AddRolePermissionAsync(RolePermission rolePermission)
        {
            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRolePermissionAsync(long roleId, long permissionId, string moduleName)
        {
            var rp = await _context.RolePermissions
                .FirstOrDefaultAsync(x => x.RoleId == roleId && x.PermissionId == permissionId && x.ModuleName == moduleName);
            if (rp != null)
            {
                _context.RolePermissions.Remove(rp);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasPermissionAsync(long roleId, string moduleName, string permissionName) =>
            await _context.RolePermissions
                .Include(rp => rp.Permission)
                .AnyAsync(rp => rp.RoleId == roleId && rp.ModuleName == moduleName && rp.Permission.Name == permissionName);
    }
}
