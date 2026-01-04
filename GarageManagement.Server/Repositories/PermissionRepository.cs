using GarageManagement.Server.Data;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;
        public PermissionRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<Permission>> GetAllPermissionsAsync() =>
            await _context.Permissions.ToListAsync();

        public async Task<Permission> GetPermissionByIdAsync(long id) =>
            await _context.Permissions.FindAsync(id);

        public async Task AddPermissionAsync(Permission permission)
        {
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePermissionAsync(Permission permission)
        {
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePermissionAsync(long id)
        {
            var perm = await _context.Permissions.FindAsync(id);
            if (perm != null)
            {
                _context.Permissions.Remove(perm);
                await _context.SaveChangesAsync();
            }
        }
    }
}
