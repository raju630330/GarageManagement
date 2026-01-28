using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Controllers
{
    public class SparePartsIssueDetailsController : BaseAuthorizationController
    {
        private readonly ApplicationDbContext _context;

        public SparePartsIssueDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateSpareParts")]
        public async Task<IActionResult> CreateSpareParts(
     [FromBody] List<SparePartsIssueDetailDto> parts)
        {
            if (parts == null || !parts.Any())
                return BadRequest("No spare parts provided.");

            var repairOrderId = parts.First().RepairOrderId;

            // 🔴 DELETE PREVIOUS SPARE PARTS FOR THIS REPAIR ORDER
            var existingParts = await _context.SparePartsIssueDetails
                .Where(x => x.RepairOrderId == repairOrderId)
                .ToListAsync();

            if (existingParts.Any())
                _context.SparePartsIssueDetails.RemoveRange(existingParts);

            // 🟢 ADD NEW SPARE PARTS
            var entities = parts.Select(p => new SparePartsIssueDetail
            {
                // ⚠️ Do NOT set Id (let DB generate it)
                RepairOrderId = p.RepairOrderId,
                Description = p.Description,
                PartNumber = p.PartNumber,
                UnitCost = p.UnitCost,
                Quantity = p.Quantity,
                Make = p.Make
            }).ToList();

            await _context.SparePartsIssueDetails.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Spare parts saved successfully!",
                count = entities.Count
            });
        }

        [HttpGet("{repairOrderId}")]
        public async Task<IActionResult> GetSpareParts(long repairOrderId)
        {
            var parts = await _context.SparePartsIssueDetails
                .Where(x => x.RepairOrderId == repairOrderId)
                .Select(x => new SparePartsIssueDetailDto
                {
                    Id = x.Id,
                    RepairOrderId = x.RepairOrderId,
                    Description = x.Description,
                    PartNumber = x.PartNumber,
                    UnitCost = x.UnitCost,
                    Quantity = x.Quantity,
                    Make = x.Make
                })
                .ToListAsync();

            if (!parts.Any())
                return Ok(new List<SparePartsIssueDetailDto>());

            return Ok(parts);
        }


    }
}
