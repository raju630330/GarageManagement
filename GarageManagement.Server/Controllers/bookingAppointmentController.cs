using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            // Map DTO to entity
            var bookingAppointment = new BookAppointment
            {
                AppointmentDate = dto.AppointmentDate,
                AppointmentTime = dto.AppointmentTime,
                Bay = dto.Bay,
                CustomerId = dto.CustomerId,
                CustomerType = dto.CustomerType,
                Service = dto.Service,
                ServiceAdvisor = dto.ServiceAdvisor,
                UserId = dto.UserId,
                VehicleId = dto.VehicleId,
                WorkshopId = dto.WorkshopId
            };

            _context.BookAppointments.Add(bookingAppointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking Appointment saved successfully", bookingAppointment.Id });
        }

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

    }
}
