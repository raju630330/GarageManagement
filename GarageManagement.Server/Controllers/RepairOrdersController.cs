using GarageManagement.Server.Controllers;
using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
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

        [HttpPost("createRepairOrder")]
        public async Task<IActionResult> CreateRepairOrder([FromBody] RepairOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var repairOrder = new RepairOrder
            {
                RegistrationNumber = dto.RegistrationNumber,
                VinNumber = dto.VinNumber,
                Mkls = dto.Mkls,
                Date = dto.Date,
                
                Make = dto.Make,
                Phone = dto.Phone,
                VehicleSite = dto.VehicleSite,
                SiteInchargeName = dto.SiteInchargeName,
                UnderWarranty = dto.UnderWarranty,
                ExpectedDateTime = dto.ExpectedDateTime,
                AllottedTechnician = dto.AllottedTechnician,
                BookingAppointmentId = dto.BookingAppointmentId
            };

            await _context.RepairOrders.AddAsync(repairOrder);
            await _context.SaveChangesAsync();

            var response = new
            {
                Id = repairOrder.Id,
                RegistrationNumber = repairOrder.RegistrationNumber,
                VinNumber = repairOrder.VinNumber,
                Date = repairOrder.Date,
                Phone = repairOrder.Phone,
                BookingAppointmentId = repairOrder.BookingAppointmentId
            };

            return Ok(new
            {
                Message = "Repair Order Created Successfully",
                Order = response
            });
        }


        [HttpGet]
        public IActionResult GetOrders()
        {
            return Ok(_context.RepairOrders.ToList());
        }
    }
}
