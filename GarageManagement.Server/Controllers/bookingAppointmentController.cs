using GarageManagement.Server.Data;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Controllers
{
    public class bookingAppointmentController : BaseAuthorizationController
    {
        private readonly ApplicationDbContext _context;

        public bookingAppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("saveBookAppointment")]

        public async Task<IActionResult> saveBookAppointment([FromBody] BookAppointment bookingAppointmentdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.bookAppointments.Add(bookingAppointmentdata);
            await _context.SaveChangesAsync();
            return Ok(new { message = " Booking Appointment Data saved successfully" });
        }

        [HttpGet("getAllAppointments")]
        public async Task<IActionResult> GetAllBookingAppointments()
        {
            var bookingAppointments = await _context.bookAppointments.ToListAsync();
            return Ok(bookingAppointments);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBookingAppointment(int id)
        {
            var bookingAppointment = await _context.bookAppointments.FindAsync(id);
            if (bookingAppointment == null)
            {
                return NotFound(new { message = "Booking Appointment not found" });
            }
            _context.bookAppointments.Remove(bookingAppointment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Booking Appointment deleted successfully" });
        }
    }
}
