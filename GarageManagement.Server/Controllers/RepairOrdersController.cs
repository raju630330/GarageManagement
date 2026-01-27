using GarageManagement.Server.Controllers;
using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GarageManagement.Api.Controllers
{
    public class RepairOrdersController : BaseAuthorizationController
    {
        private readonly ApplicationDbContext _context;
        private readonly IAutoCompleteRepository _autoCompleteRepository;

        public RepairOrdersController(ApplicationDbContext context,IAutoCompleteRepository autoCompleteRepository)
        {
            _context = context;
            _autoCompleteRepository = autoCompleteRepository;   
        }

        [HttpPost("createRepairOrder")]
        public async Task<IActionResult> CreateRepairOrder([FromBody] RepairOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var repairorder = await _context.RepairOrders.Where(a => a.Id == dto.Id).FirstOrDefaultAsync();

            if (repairorder == null) { repairorder = new RepairOrder(); }

                repairorder.RegistrationNumber = dto.RegistrationNumber;
                repairorder.VinNumber = dto.VinNumber;
                repairorder.Mkls = dto.Mkls;
                repairorder.Date = dto.Date;                
                repairorder.Make = dto.Make;
                repairorder.Phone = dto.Phone;
                repairorder.VehicleSite = dto.VehicleSite;
                repairorder.SiteInchargeName = dto.SiteInchargeName;
                repairorder.UnderWarranty = dto.UnderWarranty;
                repairorder.ExpectedDateTime = dto.ExpectedDateTime;
                repairorder.AllottedTechnician = dto.AllottedTechnician;
                repairorder.Model = dto.Model;
                repairorder.DriverName = dto.DriverName;
                repairorder.RepairEstimationCost = dto.RepairEstimationCost;
                repairorder.DriverPermanetToThisVehicle = dto.DriverPermanetToThisVehicle;
                repairorder.TypeOfService = dto.TypeOfService;
                repairorder.RoadTestAlongWithDriver = dto.RoadTestAlongWithDriver;
                repairorder.BookingAppointmentId = dto.BookingAppointmentId;

            if (dto.Id == 0)
            {
                await _context.RepairOrders.AddAsync(repairorder);
            }
            else
            {
                _context.RepairOrders.Update(repairorder);
            }
            await _context.SaveChangesAsync();

            var response = new
            {
                Id = repairorder.Id,
                RegistrationNumber = repairorder.RegistrationNumber,
                VinNumber = repairorder.VinNumber,
                Date = repairorder.Date,
                Phone = repairorder.Phone,
                BookingAppointmentId = repairorder.BookingAppointmentId
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

        [HttpGet("by-booking/{bookingId}")]
        public async Task<IActionResult> GetByBooking(int bookingId)
        {
            var order = await _context.RepairOrders
                .Where(x => x.BookingAppointmentId == bookingId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (order == null)
                return Ok(new { exists = false });

            return Ok(new
            {
                exists = true,
                repairOrderId = order.Id,
                data = new
                {
                    registrationNumber = order.RegistrationNumber,
                    vinNumber = order.VinNumber,
                    mkls = order.Mkls,
                    date = order.Date,
                    make = order.Make,
                    phone = order.Phone,
                    vehicleSite = order.VehicleSite,
                    siteInchargeName = order.SiteInchargeName,

                    underWarranty = order.UnderWarranty,
                    expectedDateTime = order.ExpectedDateTime,
                    allottedTechnician = order.AllottedTechnician,

                    model = order.Model,
                    driverName = order.DriverName,
                    repairEstimationCost = order.RepairEstimationCost,

                    driverPermanetToThisVehicle = order.DriverPermanetToThisVehicle,
                    typeOfService = order.TypeOfService,
                    roadTestAlongWithDriver = order.RoadTestAlongWithDriver
                }
            });
        }

        [HttpGet("search-booking-appointment")]
        public async Task<IActionResult> SearchBookingAppointment(string query)
        {
            return Ok(await _autoCompleteRepository.SearchBookingAppointment(query));
        }

    }
}
