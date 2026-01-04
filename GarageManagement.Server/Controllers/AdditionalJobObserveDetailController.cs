using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> CreateAdditionalJobObserveDetails([FromBody] List<AdditionalJobObserveDetailDto> details)
        {
            if (details == null || !details.Any())
            {
                return BadRequest("No Additional Job Observe Details Found");
            }

            // Map DTO to Entity
            var entities = details.Select(d => new AdditionalJobObserveDetail
            {
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

    }
}
