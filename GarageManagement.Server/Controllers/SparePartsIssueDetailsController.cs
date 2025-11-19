using GarageManagement.Server.Data;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SparePartsIssueDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SparePartsIssueDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateSpareParts")]
        public async Task<IActionResult> CreateSpareParts([FromBody] List<SparePartsIssueDetail> parts)
        {
            if (parts == null || !parts.Any())
                return BadRequest("No spare parts provided.");

            await _context.SparePartsIssueDetails.AddRangeAsync(parts);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Spare parts saved successfully!" });
        }
    }
}
