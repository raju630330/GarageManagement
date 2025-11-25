using GarageManagement.Server.Data;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Server.Controllers
{   
    public class TechnicianMCController : BaseAuthorizationController
    {
        private readonly ApplicationDbContext _context;

        public TechnicianMCController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("save")]
        public IActionResult SaveTechnicianForm([FromBody] TechnicianMC model)
        {
            if (model == null)
                return BadRequest("Invalid form data.");

            var form = new TechnicianMC
            {
                Remarks = model.Remarks,
                TechnicianSign = model.TechnicianSign,
                DriverSign = model.DriverSign,
                FloorSign = model.FloorSign,
                AuthSign = model.AuthSign,
                CheckList = model.CheckList?.Select(c => new CheckItemEntity
                {
                    Label = c.Label,
                    Control = c.Control,
                    Status = c.Status
                }).ToList()
            };

            _context.TechnicianMCForms.Add(form);
            _context.SaveChanges();

            return Ok(new { message = "Technician form saved successfully!" });
        }
    }
    

    
}
