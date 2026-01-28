using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Controllers
{
    public class TechnicianMCController : BaseAuthorizationController
    {
        private readonly ApplicationDbContext _context;

        public TechnicianMCController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 Save or Update Technician Form
        [HttpPost("save")]
        public async Task<IActionResult> SaveTechnicianForm([FromBody] TechnicianMCDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var form = await _context.TechnicianMCForms.Include(x => x.CheckList).Where(a=> a.Id == dto.Id).FirstOrDefaultAsync();  


            if (form != null && form.CheckList.Any())
            {
                _context.CheckItems.RemoveRange(form.CheckList);
            }                      

            if (form == null)
            {
                form = new TechnicianMC
                {
                    RepairOrderId = dto.RepairOrderId,
                    CheckList = new List<CheckItemEntity>()
                };
            }
                
            form.Remarks = dto.Remarks;
            form.TechnicianSign = dto.TechnicianSign;
            form.DriverSign = dto.DriverSign;
            form.FloorSign = dto.FloorSign;
            form.AuthSign = dto.AuthSign;
            form.CheckList = dto.CheckList?.Select(c => new CheckItemEntity
            {
                Label = c.Label,
                Control = c.Control,
                Status = c.Status
            }).ToList();

            if (dto.Id == 0)
            {
                await _context.TechnicianMCForms.AddAsync(form);
            }
            else
            { 
                _context.TechnicianMCForms.Update(form);              
            }
            await _context.SaveChangesAsync();

            return Ok(new { id = form.Id, message = "Technician form saved successfully!" });
        }

        // 🔹 Get Technician Form by RepairOrderId
        [HttpGet("{repairOrderId}")]
        public async Task<IActionResult> GetTechnicianForm(long repairOrderId)
        {
            var form = await _context.TechnicianMCForms
                .Include(x => x.CheckList)
                .Where(x => x.RepairOrderId == repairOrderId)
                .Select(x => new TechnicianMCDto
                {
                    Id = x.Id,  
                    RepairOrderId = x.RepairOrderId,
                    Remarks = x.Remarks,
                    TechnicianSign = x.TechnicianSign,
                    DriverSign = x.DriverSign,
                    FloorSign = x.FloorSign,
                    AuthSign = x.AuthSign,
                    CheckList = x.CheckList.Select(c => new CheckItemDto
                    {
                        Label = c.Label,
                        Control = c.Control,
                        Status = c.Status
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (form == null)
                return Ok(null);

            return Ok(form);
        }
    }
}
