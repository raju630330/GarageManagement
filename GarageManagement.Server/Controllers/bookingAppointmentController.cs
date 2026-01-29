using Castle.Core.Resource;
using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;

namespace GarageManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingAppointmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAutoCompleteRepository _autoCompleteRepository;

        public BookingAppointmentController(ApplicationDbContext context, IAutoCompleteRepository autoCompleteRepository)
        {
            _context = context;
            _autoCompleteRepository = autoCompleteRepository;   
        }

        // ================================
        // POST: api/BookingAppointment/saveBookAppointment
        // ================================
        [HttpPost("saveBookAppointment")]


        public async Task<IActionResult> SaveBookAppointment([FromBody] BookingAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Resolve Customer
            Customer customer = dto.CustomerId > 0
                ? await _context.Customers.FindAsync(dto.CustomerId)
                : new Customer
                {
                    CustomerName = dto.CustomerName,
                    MobileNo = dto.MobileNo,
                    Email = dto.Email,
                    CustomerType = dto.CustomerType
                };

            if (customer == null)
                return BadRequest("Customer not found");

            // Resolve Vehicle
            Vehicle? vehicle = dto.VehicleId > 0
                ? await _context.Vehicles.FindAsync(dto.VehicleId)
                : string.IsNullOrWhiteSpace(dto.RegNo)
                    ? null
                    : new Vehicle
                    {
                        RegPrefix = dto.RegPrefix,
                        RegNo = dto.RegNo,
                        VehicleType = dto.VehicleType,
                        Customer = customer
                    };

            if (dto.VehicleId > 0 && vehicle == null)
                return BadRequest("Vehicle not found");

            // Resolve Booking
            var booking = await _context.BookAppointments
                .FirstOrDefaultAsync(x => x.Id == dto.Id) ?? new BookAppointment();

            booking.AppointmentDate = dto.AppointmentDate;
            booking.AppointmentTime = dto.AppointmentTime;
            booking.Bay = dto.Bay;
            booking.Customer = customer;
            booking.Vehicle = vehicle;
            booking.CustomerType = dto.CustomerType;
            booking.Service = dto.Service;
            booking.ServiceAdvisor = dto.ServiceAdvisor;
            booking.UserId = dto.UserId;
            booking.WorkshopId = dto.WorkshopId;

            if (dto.Id == 0)
                _context.BookAppointments.Add(booking);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Saved successfully",
                booking.Id,
                CustomerId = customer.Id,
                VehicleId = vehicle?.Id
            });
        }

        /* public async Task<IActionResult> SaveBookAppointment([FromBody] BookingAppointmentDto dto)
         {
             if (!ModelState.IsValid)
                 return BadRequest(ModelState);

             // Validate CustomerId exists
             var customerExists = await _context.Customers.AnyAsync(c => c.Id == dto.CustomerId);
             if (!customerExists)
                 return BadRequest(new { message = "Customer not found" });

             // Validate VehicleId if provided
             if (dto.VehicleId != null)
             {
                 var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == dto.VehicleId);
                 if (!vehicleExists)
                     return BadRequest(new { message = "Vehicle not found" });
             }

             var result = await _context.BookAppointments.Where(a => a.Id == dto.Id).FirstOrDefaultAsync();

             if (result == null)
             {
                 result = new BookAppointment();
             }

             result.AppointmentDate = dto.AppointmentDate;
             result.AppointmentTime = dto.AppointmentTime;
                 result.Bay = dto.Bay;
             result.CustomerId = dto.CustomerId;
             result.CustomerType = dto.CustomerType;
                 result.Service = dto.Service;
             result.ServiceAdvisor = dto.ServiceAdvisor;
             result.UserId = dto.UserId;
             result.VehicleId = dto.VehicleId;
             result.WorkshopId = dto.WorkshopId;

             if (dto.Id == 0)
             {
                 await _context.BookAppointments.AddAsync(result);
             }
             else
             {
                 _context.BookAppointments.Update(result);
             }


             // Map DTO to entity

             await _context.SaveChangesAsync();

             return Ok(new { message = "Booking Appointment saved successfully", result.Id });
         }*/

        // ================================
        // GET: api/BookingAppointment/getAllAppointments
        // ================================
        [HttpGet("getAllAppointments")]
        public async Task<IActionResult> GetAllBookingAppointments()
        {
            // Include related Customer and Vehicle info for display
            var bookingAppointments = await _context.BookAppointments
                .Include(b => b.Customer)
                .Include(b => b.Vehicle)
                .Include(b => b.Workshop)
                .ToListAsync();

            var result = bookingAppointments.Select(b => new
            {
                b.Id,
                b.AppointmentDate,
                b.AppointmentTime,
                b.CustomerType,
                b.Service,
                b.ServiceAdvisor,
                b.Bay,
                CustomerName = b.Customer.CustomerName,
                CustomerMobile = b.Customer.MobileNo,
                VehicleRegNo = b.Vehicle != null ? b.Vehicle.RegPrefix + b.Vehicle.RegNo : null,
                b.UserId,
                WorkshopName = b.Workshop != null ? b.Workshop.WorkshopName : null
            });

            return Ok(result);
        }

        // ================================
        // DELETE: api/BookingAppointment/{id}
        // ================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookingAppointment(long id)
        {
            var bookingAppointment = await _context.BookAppointments.FindAsync(id);
            if (bookingAppointment == null)
            {
                return NotFound(new { message = "Booking Appointment not found" });
            }

            _context.BookAppointments.Remove(bookingAppointment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Booking Appointment deleted successfully" });
        }

        [HttpGet("search-booking-customer")]
        public async Task<IActionResult> SearchBookingCustomer([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query is required");

            var result = await _autoCompleteRepository.SearchBookingCustomer(query);
            return Ok(result);
        }

        [HttpGet("getCustomerDetails/{id}")]
        public async Task<IActionResult> GetCustomerDetails(long id)
        {
            var customer = await _context.Customers
                .Include(c => c.Vehicles)
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.CustomerName,
                    c.MobileNo,
                    c.Email,
                    c.CustomerType,
                    Vehicles = c.Vehicles.Select(v => new
                    {
                        v.Id,
                        v.RegPrefix,
                        v.RegNo,
                        v.VehicleType
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpGet("getBooking/{id}")]
        public IActionResult GetBookingById(int id)
        {
            var booking = _context.BookAppointments
                .Where(x => x.Id == id)
                .Select(x => new 
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    VehicleId = x.VehicleId,
                    AppointmentDate = x.AppointmentDate,
                    AppointmentTime = x.AppointmentTime,
                    CustomerType = x.CustomerType,
                    RegNo = x.Vehicle.RegNo,
                    CustomerName = x.Customer.CustomerName,
                    MobileNo = x.Customer.MobileNo,
                    EmailID = x.Customer.Email,
                    VehicleType = x.Vehicle.VehicleType,
                    RegPrefix = x.Vehicle.RegPrefix,
                    Service = x.Service,
                    ServiceAdvisor = x.ServiceAdvisor,
                    Bay = x.Bay,
                    WorkshopId = x.WorkshopId,
                    UserId = x.UserId
                })
                .FirstOrDefault();

            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            return Ok(booking);
        }

    }
}
