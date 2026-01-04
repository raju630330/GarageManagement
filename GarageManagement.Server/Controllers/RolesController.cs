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
        public async Task<ActionResult<List<Role>>> GetAll()
        {
            var roles = await _roleRepo.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> Get(long id)
        {
            var role = await _roleRepo.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Role role)
        {
            await _roleRepo.AddRoleAsync(role);
            return CreatedAtAction(nameof(Get), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, [FromBody] Role role)
        {
            if (id != role.Id) return BadRequest();
            await _roleRepo.UpdateRoleAsync(role);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _roleRepo.DeleteRoleAsync(id);
            return NoContent();
        }
    }
}
