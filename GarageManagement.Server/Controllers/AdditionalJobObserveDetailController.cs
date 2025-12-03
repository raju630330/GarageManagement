using GarageManagement.Server.Data;
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
        public async Task<ActionResult> CreateAdditionalJobObserveDetails([FromBody]List<AdditionalJobObserveDetail> details)
        {
            if (!details.Any() || details == null)
            {
                return BadRequest("No Additional Job Observe Details Found");
            }
            await _context.AdditionalJobObserveDetail.AddRangeAsync(details);   
            await _context.SaveChangesAsync();
            return Ok(new { message = "Additional Job Observe Details Saved Sucessfully" });                     
        }
    }
}
