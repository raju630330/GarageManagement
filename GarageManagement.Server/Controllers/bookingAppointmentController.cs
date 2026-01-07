using GarageManagement.Server.Data;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingAppointmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingAppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================================
        // POST: api/BookingAppointment/saveBookAppointment
        // ================================
        [HttpPost("saveBookAppointment")]
        public async Task<IActionResult> SaveBookAppointment([FromBody] BookAppointment bookingAppointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Optional: Validate CustomerId and VehicleId exist
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == bookingAppointment.CustomerId);
            if (!customerExists)
                return BadRequest(new { message = "Customer not found" });

            if (bookingAppointment.VehicleId != null)
            {
                var vehicleExists = await _context.Vehicles.AnyAsync(v => v.Id == bookingAppointment.VehicleId);
                if (!vehicleExists)
                    return BadRequest(new { message = "Vehicle not found" });
            }

            _context.BookAppointments.Add(bookingAppointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking Appointment Data saved successfully", bookingAppointment.Id });
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
    }
}
