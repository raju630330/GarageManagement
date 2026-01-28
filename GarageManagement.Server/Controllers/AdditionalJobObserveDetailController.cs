using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Controllers
{
    public class AdditionalJobObserveDetailController : BaseAuthorizationController
    {
        private readonly ApplicationDbContext _context;

        public AdditionalJobObserveDetailController(ApplicationDbContext context)
        {         
          _context = context;  
        }

        [HttpPost("additionaljobobservedetails")]
        public async Task<ActionResult> CreateAdditionalJobObserveDetails(
            [FromBody] List<AdditionalJobObserveDetailDto> details)
        {
            if (details == null || !details.Any())
            {
                return BadRequest("No Additional Job Observe Details Found");
            }

            var repairOrderId = details.First().RepairOrderId;

            // 🔥 DELETE existing records for this RepairOrder
            var existingRecords = await _context.AdditionalJobObserveDetails
                .Where(x => x.RepairOrderId == repairOrderId)
                .ToListAsync();

            if (existingRecords.Any())
            {
                _context.AdditionalJobObserveDetails.RemoveRange(existingRecords);
            }

            // 🔥 INSERT new records
            var entities = details.Select(d => new AdditionalJobObserveDetail
            {
                RepairOrderId = d.RepairOrderId,
                TechnicianVoice = d.TechnicianVoice,
                SupervisorInstructions = d.SupervisorInstructions,
                ActionTaken = d.ActionTaken,
                StartTime = d.StartTime,
                EndTime = d.EndTime
            }).ToList();

            await _context.AdditionalJobObserveDetails.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Additional Job Observe Details Saved Successfully" });
        }


    [HttpGet("getadditionaljobobservedetails/{repairOrderId}")]
    public async Task<ActionResult> GetAdditionalJobObserveDetails(int repairOrderId)
    {
        var details = await _context.AdditionalJobObserveDetails
            .Where(x => x.RepairOrderId == repairOrderId)
            .OrderBy(x => x.Id)
            .Select(x => new AdditionalJobObserveDetailDto
            {
                RepairOrderId = x.RepairOrderId,
                TechnicianVoice = x.TechnicianVoice,
                SupervisorInstructions = x.SupervisorInstructions,
                ActionTaken = x.ActionTaken,
                StartTime = x.StartTime,
                EndTime = x.EndTime
            })
            .ToListAsync();

        if (!details.Any())
        {
            return Ok(new List<AdditionalJobObserveDetailDto>()); // return empty array
        }

        return Ok(details);
    }


}
}
