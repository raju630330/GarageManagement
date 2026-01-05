using GarageManagement.Server.dtos;
using GarageManagement.Server.RepoInterfaces;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarageManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IRolePermissionRepository _rolePermRepo;

        public RolePermissionsController(IRolePermissionRepository rolePermRepo)
        {
            _rolePermRepo = rolePermRepo;
        }

        // ==============================
        // MODULE MANAGEMENT
        // ==============================

        // Add a new permission module dynamically
        [HttpPost("module")]
        public async Task<ActionResult<BaseResultDto>> AddModule([FromBody] PermissionModule module)
        {
            if (module == null || string.IsNullOrWhiteSpace(module.Name))
                return BadRequest(new BaseResultDto { IsSuccess = false, Message = "Module name is required" });

            var result = await _rolePermRepo.AddModuleAsync(module.Name, module.Description);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Get all modules
        [HttpGet("modules")]
        public async Task<ActionResult<List<PermissionModule>>> GetAllModules()
        {
            var modules = await _rolePermRepo.GetAllModulesAsync();
            return Ok(modules);
        }

        // ==============================
        // ROLE PERMISSION MANAGEMENT
        // ==============================

        // Get all permissions for a role
        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<List<RolePermissionDto>>> GetRolePermissions(long roleId)
        {
            var permissions = await _rolePermRepo.GetRolePermissionsAsync(roleId);
            return Ok(permissions);
        }

        // Get permission names for a role + module
        [HttpGet("role/{roleId}/module/{moduleId}")]
        public async Task<ActionResult<List<string>>> GetRoleModulePermissions(long roleId, long moduleId)
        {
            var allModules = await _rolePermRepo.GetAllModulesAsync();
            var module = allModules.Find(m => m.Id == moduleId);
            if (module == null)
                return NotFound(new BaseResultDto { IsSuccess = false, Message = "Module not found" });

            var permissions = await _rolePermRepo.GetRoleModulePermissionsAsync(roleId, module.Name);
            return Ok(permissions);
        }

        // Assign or update role permission
        [HttpPost("permission")]
        public async Task<ActionResult<BaseResultDto>> SaveRolePermission([FromBody] RolePermissionDto dto)
        {
            var result = await _rolePermRepo.SaveRolePermissionAsync(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Remove a single permission for a role + module
        [HttpDelete("permission")]
        public async Task<ActionResult<BaseResultDto>> RemoveRolePermission(long roleId, long permissionId, long moduleId)
        {
            var result = await _rolePermRepo.RemoveRolePermissionAsync(roleId, permissionId, moduleId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Clear all permissions for a role + module
        [HttpDelete("role/{roleId}/module/{moduleId}/clear")]
        public async Task<ActionResult<BaseResultDto>> ClearRoleModulePermissions(long roleId, long moduleId)
        {
            var result = await _rolePermRepo.ClearRoleModulePermissionsAsync(roleId, moduleId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        // Check if role has a specific permission
        [HttpGet("check")]
        public async Task<ActionResult<bool>> HasPermission(long roleId, long moduleId, string permissionName)
        {
            var modules = await _rolePermRepo.GetAllModulesAsync();
            var module = modules.Find(m => m.Id == moduleId);
            if (module == null)
                return NotFound(false);

            var hasPermission = await _rolePermRepo.HasPermissionAsync(roleId, module.Name, permissionName);
            return Ok(hasPermission);
        }
    }
}
