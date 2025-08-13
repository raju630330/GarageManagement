using GarageManagement.Server.Data;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkshopProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkshopProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("save")]
        public async Task<IActionResult> save([FromBody] WorkshopProfile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.WorkshopProfiles.Add(profile);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Profile saved successfully" });
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetProfile(int id)
        //{
        //    var profile = await _context.WorkshopProfiles.FindAsync(id);
        //    if (profile == null)
        //        return NotFound();

        //    return Ok(profile);
        //}

        
    }
}
