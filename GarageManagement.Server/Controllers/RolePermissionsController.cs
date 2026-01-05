using GarageManagement.Server.dtos;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarageManagement.Server.Controllers
{
    public class RolePermissionsController : BaseAuthorizationController
    {
        private readonly IRolePermissionRepository _rolePermRepo;

        public RolePermissionsController(IRolePermissionRepository rolePermRepo)
        {
            _rolePermRepo = rolePermRepo;
        }

        [HttpGet("{roleId}/{moduleName}")]
        public async Task<ActionResult<List<string>>> GetRoleModulePermissions(long roleId, string moduleName)
        {
            var permissions = await _rolePermRepo.GetRoleModulePermissionsAsync(roleId, moduleName);
            return Ok(permissions);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResultDto>> Save([FromBody] RolePermissionDto dto)
        {
            var result = await _rolePermRepo.SaveRolePermissionAsync(dto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete]
        public async Task<ActionResult<BaseResultDto>> Remove(long roleId, long permissionId, string moduleName)
        {
            var result = await _rolePermRepo.RemoveRolePermissionAsync(roleId, permissionId, moduleName);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("clear")]
        public async Task<ActionResult<BaseResultDto>> ClearRoleModulePermissions(long roleId, string moduleName)
        {
            var result = await _rolePermRepo.ClearRoleModulePermissionsAsync(roleId, moduleName);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
