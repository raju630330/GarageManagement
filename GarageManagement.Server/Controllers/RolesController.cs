using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Server.Controllers
{
    public class RolesController : BaseAuthorizationController
    {
        private readonly IRoleRepository _roleRepo;

        public RolesController(IRoleRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoleModel>>> GetAll()
        {
            var roles = await _roleRepo.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleModel>> Get(long id)
        {
            var role = await _roleRepo.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Save(RoleModel role)
        {
            var result = await _roleRepo.AddRoleAsync(role);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _roleRepo.DeleteRoleAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
