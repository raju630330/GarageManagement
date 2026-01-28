using GarageManagement.Server.Controllers;
using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class LabourDetailsController : BaseAuthorizationController
{
    private readonly ApplicationDbContext _context;

    public LabourDetailsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetByRepairOrder/{repairOrderId}")]
    public async Task<IActionResult> GetByRepairOrder(int repairOrderId)
    {
        var data = await _context.LabourDetails
            .Where(x => x.RepairOrderId == repairOrderId)
            .Select(d => new LabourDetailDto() {
                RepairOrderId = d.RepairOrderId,
                Description = d.Description,
                LabourCharges = d.LabourCharges,
                OutsideLabour = d.OutsideLabour,
            })
            .ToListAsync();

        return Ok(data);
    }

    [HttpPost("CreateLabourDetails")]
    public async Task<IActionResult> CreateLabourDetails(
        [FromBody] List<LabourDetailDto> details)
    {
        if (details == null || !details.Any())
            return BadRequest("No labour details provided.");

        var repairOrderId = details.First().RepairOrderId;

        var existing = await _context.LabourDetails
            .Where(x => x.RepairOrderId == repairOrderId)
            .ToListAsync();

        if (existing.Any())
        {
            _context.LabourDetails.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }

        var entities = details.Select(d => new LabourDetail
        {
            RepairOrderId = d.RepairOrderId,
            Description = d.Description,
            LabourCharges = d.LabourCharges,
            OutsideLabour = d.OutsideLabour,
            Amount = d.LabourCharges + d.OutsideLabour
        }).ToList();

        await _context.LabourDetails.AddRangeAsync(entities);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Labour details saved successfully!",
            count = entities.Count
        });
    }
}
