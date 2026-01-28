using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                return BadRequest(new { message = "No supervisor details provided." });

            // Get the RepairOrderId from the first model (assuming all models belong to the same order)
            var repairOrderId = models.First().RepairOrderId;

            // Remove existing supervisor records for this RepairOrderId
            var existingRecords = _context.ToBeFilledBySupervisors
                .Where(x => x.RepairOrderId == repairOrderId);

            if (existingRecords.Any())
            {
                _context.ToBeFilledBySupervisors.RemoveRange(existingRecords);
            }

            // Map DTOs to entity
            var entities = models.Select(d => new ToBeFilledBySupervisor
            {
                DriverVoice = d.DriverVoice,
                SupervisorInstructions = d.SupervisorInstructions,
                ActionTaken = d.ActionTaken,
                StartTime = d.StartTime,
                EndTime = d.EndTime,
                RepairOrderId = d.RepairOrderId
            }).ToList();

            await _context.ToBeFilledBySupervisors.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Supervisor section saved successfully" });
        }

        [HttpGet("get/{repairOrderId}")]
        public async Task<IActionResult> GetByRepairOrderId(long repairOrderId)
        {
            var records = await _context.ToBeFilledBySupervisors
                                        .Where(x => x.RepairOrderId == repairOrderId)
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.DriverVoice,
                                            x.SupervisorInstructions,
                                            x.ActionTaken,
                                            x.StartTime,
                                            x.EndTime,
                                            x.RepairOrderId
                                        })
                                        .ToListAsync();

            if (!records.Any())
                return NotFound(new { message = "No supervisor details found for this repair order." });

            return Ok(records);
        }

    }
}