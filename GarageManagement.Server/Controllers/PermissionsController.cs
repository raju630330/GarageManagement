using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Server.Controllers
{
    public class PermissionsController : BaseAuthorizationController
    {
        private readonly IPermissionRepository _permissionRepo;

        public PermissionsController(IPermissionRepository permissionRepo)
        {
            _permissionRepo = permissionRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _permissionRepo.GetAllPermissionsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var perm = await _permissionRepo.GetPermissionByIdAsync(id);
            if (perm == null) return NotFound();
            return Ok(perm);
        }

        [HttpPost]
        public async Task<IActionResult> Save(PermissionDto permission)
        {
            var result = await _permissionRepo.SavePermissionAsync(permission);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _permissionRepo.DeletePermissionAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

    }
}

