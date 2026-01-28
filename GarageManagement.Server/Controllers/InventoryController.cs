using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GarageManagement.Server.Controllers
{
    public class InventoryController : BaseAuthorizationController
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpPost("save")]
        public async Task<IActionResult> SaveInventory([FromBody] InventoryDto data)
        {
            if (data == null || data.Accessories == null || !data.Accessories.Any())
                return BadRequest(new { message = "Please select at least one accessory." });

            var result = await _context.InventoryForms.Include(a=>a.Accessories).Where(a => a.Id == data.Id).FirstOrDefaultAsync();

            if (result!=null && result.Accessories.Any())
            { 
                 _context.InventoryAccessories.RemoveRange(result.Accessories);    
            }

            if (result == null)
            {
                result = new InventoryForm();
                result.Accessories = new List<InventoryAccessory>();
            }

            result.Id = data.Id;
            result.RepairOrderId = data.RepairOrderId;
            result.Accessories = data.Accessories.Select(x => new InventoryAccessory
            {
                Label = x.Label,
                Checked = x.Checked,
            }).ToList();

            if (data.Id == 0)
            {
                await _context.InventoryForms.AddAsync(result);

            }
            else
            {
                _context.InventoryForms.Update(result);
            }


            await _context.SaveChangesAsync();

            return Ok(new { id = result.Id, message = "Inventory saved successfully!" });
        }
        [HttpGet("get/{repairOrderId}")]
        public IActionResult GetInventoryByRepairOrderId(long repairOrderId)
        {
            var inventory = _context.InventoryForms
                .Where(f => f.RepairOrderId == repairOrderId)
                .Select(f => new
                {
                    Id = f.Id,  
                    RepairOrderId = repairOrderId,
                    Accessories = f.Accessories
                        .Select(a => new 
                        {
                            Label = a.Label,
                            Checked = a.Checked
                        })
                        .ToList()
                })
                .FirstOrDefault();

            if (inventory == null)
                return NotFound(new { message = "Inventory not found for this Repair Order." });

            return Ok(inventory);
        }

    }
}
