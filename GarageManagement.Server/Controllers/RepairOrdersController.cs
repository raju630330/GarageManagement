using GarageManagement.Server.Controllers;
using GarageManagement.Server.Data;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Api.Controllers
{
    public class RepairOrdersController : BaseAuthorizationController
    {
        private readonly ApplicationDbContext _context;

        public RepairOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateRepairOrder([FromBody] RepairOrder order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.RepairOrders.Add(order);
            _context.SaveChanges();

            return Ok(new { Message = "Repair Order Created Successfully", Order = order });
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            return Ok(_context.RepairOrders.ToList());
        }
    }
}
