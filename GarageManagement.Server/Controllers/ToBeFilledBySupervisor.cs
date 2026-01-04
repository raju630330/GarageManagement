using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToBeFilledBySupervisorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ToBeFilledBySupervisorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("savedetails")]
        public async Task<IActionResult> Create([FromBody] List<ToBeFilledBySupervisorDto> models)
        {
            if (models == null || !models.Any())
            {
                return BadRequest(new { message = "No supervisor details provided." });
            }

            // Map DTOs to entity
            var entities = models.Select(d => new ToBeFilledBySupervisor
            {
                DriverVoice = d.DriverVoice,
                SupervisorInstructions = d.SupervisorInstructions,
                ActionTaken = d.ActionTaken,
                StartTime = d.StartTime,
                EndTime = d.EndTime
            }).ToList();

            await _context.ToBeFilledBySupervisors.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Supervisor section saved successfully" });
        }
    }
}