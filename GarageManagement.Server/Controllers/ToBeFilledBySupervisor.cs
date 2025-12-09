using Microsoft.AspNetCore.Mvc;
using GarageManagement.Server.Data;
using GarageManagement.Server.Model;

namespace GarageManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToBeFilledBySupervisorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ToBeFilledBySupervisorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.ToBeFilledBySupervisor.ToList());
        }

        [HttpPost]
        public IActionResult Create([FromBody] ToBeFilledBySupervisor model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.ToBeFilledBySupervisor.Add(model);
            _context.SaveChanges();

            return Ok(new { message = "Supervisor section saved successfully" });
        }
    }
}