using GarageManagement.Server.Data;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class LabourDetailsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LabourDetailsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LabourDetail>>> GetLabourDetails()
    {
        return await _context.LabourDetails.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<LabourDetail>> PostLabourDetail(LabourDetail detail)
    {
        _context.LabourDetails.Add(detail);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLabourDetails), new { id = detail.Id }, detail);
    }
}
