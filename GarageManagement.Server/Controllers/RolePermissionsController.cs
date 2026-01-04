using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Server.Controllers
{
    public class RolePermissionsController : BaseAuthorizationController
    {
        private readonly IRolePermissionRepository _rolePermRepo;

        public RolePermissionsController(IRolePermissionRepository rolePermRepo)
        {
            _rolePermRepo = rolePermRepo;
        }

        // Get permissions for a role and module
        [HttpGet("{roleId}/{moduleName}")]
        public async Task<ActionResult<List<string>>> GetRoleModulePermissions(long roleId, string moduleName)
        {
            var permissions = await _rolePermRepo.GetRoleModulePermissionsAsync(roleId, moduleName);
            return Ok(permissions); // returns ["A", "V"]
        }

        // Add permission for a role/module
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] RolePermission rolePermission)
        {
            await _rolePermRepo.AddRolePermissionAsync(rolePermission);
            return Ok();
        }

        // Remove permission from a role/module
        [HttpDelete]
        public async Task<ActionResult> Remove(long roleId, long permissionId, string moduleName)
        {
            await _rolePermRepo.RemoveRolePermissionAsync(roleId, permissionId, moduleName);
            return NoContent();
        }
    }
}
