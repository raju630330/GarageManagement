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
        public async Task<ActionResult<List<Permission>>> GetAll()
        {
            var permissions = await _permissionRepo.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Permission>> Get(long id)
        {
            var permission = await _permissionRepo.GetPermissionByIdAsync(id);
            if (permission == null) return NotFound();
            return Ok(permission);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Permission permission)
        {
            await _permissionRepo.AddPermissionAsync(permission);
            return CreatedAtAction(nameof(Get), new { id = permission.Id }, permission);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, [FromBody] Permission permission)
        {
            if (id != permission.Id) return BadRequest();
            await _permissionRepo.UpdatePermissionAsync(permission);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _permissionRepo.DeletePermissionAsync(id);
            return NoContent();
        }
    }
}

