using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult SaveInventory([FromBody] InventoryDto data)
        {
            if (data == null || data.Accessories == null || !data.Accessories.Any())
                return BadRequest(new { message = "Please select at least one accessory." });

            var form = new InventoryForm
            {
                Accessories = data.Accessories.Select(x => new InventoryAccessory
                {
                    Label = x.Label,
                    Checked = x.Checked
                }).ToList()
            };

            _context.InventoryForms.Add(form);
            _context.SaveChanges();

            return Ok(new { message = "Inventory saved successfully!" });
        }
    }
}
