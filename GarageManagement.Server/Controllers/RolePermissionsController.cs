using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Server.Controllers
{
    [ApiController]
    [Route("api/RolePermissions")]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IRolePermissionRepository _repo;
        public RolePermissionsController(IRolePermissionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<RoleDto>>> GetRoles() =>
            Ok(await _repo.GetAllRolesAsync());

        [HttpGet("modules")]
        public async Task<ActionResult<List<PermissionModule>>> GetModules() =>
            Ok(await _repo.GetAllModulesAsync());

        [HttpPost("module")]
        public async Task<ActionResult<BaseResultDto>> AddModule([FromBody] PermissionModule module)
        {
            var result = await _repo.AddModuleAsync(module.Name);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<List<RolePermissionDto>>> GetRolePermissions(long roleId) =>
            Ok(await _repo.GetRolePermissionsAsync(roleId));

        [HttpPost("role/save")]
        public async Task<ActionResult<BaseResultDto>> SaveRolePermissions([FromBody] List<RolePermissionDto> permissions)
        {
            var result = await _repo.SaveRolePermissionsAsync(permissions);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
