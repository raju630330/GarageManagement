using GarageManagement.Server.Data;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class bookingAppointmentController : Controller
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
    }
}
