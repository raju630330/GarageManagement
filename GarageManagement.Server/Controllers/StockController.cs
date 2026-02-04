using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.RepoInterfaces;
using GarageManagement.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Controllers
{
    [Route("api/stock")]
    public class StockController : BaseAuthorizationController
    {
        private readonly ApplicationDbContext _context;
        private readonly IAutoCompleteRepository _autoCompleteRepository;
        public StockController(ApplicationDbContext context, IAutoCompleteRepository autoCompleteRepository)
        {
            _context = context;
            _autoCompleteRepository = autoCompleteRepository;
        }

        // =========================================================
        // 🔹 STOCK LIST
        // =========================================================
        [HttpGet("list")]
        public async Task<IActionResult> GetStockList(
    string stockType = "IN",
    string search = "",
    int page = 1,
    int pageSize = 10)
        {
            // Load parts with their stock movements into memory
            var parts = await _context.Parts
                .Include(p => p.StockMovements)
                .Where(p => p.IsActive)
                .ToListAsync();

            // Filter by search term if provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                parts = parts.Where(p =>
                    p.PartNo.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.PartName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Brand.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Map to DTO and calculate quantities and averages
            var stockList = parts.Select(p =>
            {
                var inwards = p.StockMovements.Where(m => m.Quantity > 0).ToList();
                var qtyOnHand = p.StockMovements.Sum(m => m.Quantity);

                return new StockListDto
                {
                    PartNo = p.PartNo,
                    PartName = p.PartName,
                    Brand = p.Brand,
                    Category = p.Category,
                    QtyOnHand = qtyOnHand,
                    AvgPurchasePrice = inwards.Any() ? inwards.Average(m => m.PurchasePrice) : 0,
                    AvgSellingPrice = inwards.Any() ? inwards.Average(m => m.SellingPrice) : 0,
                    TaxType = p.TaxType,
                    TaxPercent = p.TaxPercent,
                    TaxAmount = (inwards.Any() ? inwards.Average(m => m.SellingPrice) : 0) * p.TaxPercent / 100,
                    RackNo = p.RackNo,
                    Ageing = inwards.Any() ? inwards.Min(m => (int)(DateTime.Now - m.TransactionDate).TotalDays) : 0,
                    Barcode = p.StockMovements
                        .OrderByDescending(m => m.TransactionDate)
                        .Select(m => m.Barcode)
                        .FirstOrDefault() ?? ""
                };
            })
            .Where(x =>
                stockType == "IN" ? x.QtyOnHand > 0 :
                stockType == "OUT" ? x.QtyOnHand <= 0 :
                true)
            .OrderBy(x => x.PartName)
            .ToList();

            // 🔹 PAGINATION (NEW)
            var totalRecords = stockList.Count;

            var pagedData = stockList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                totalRecords,
                page,
                pageSize,
                items = pagedData
            });
        }


        // =========================================================
        // 🔹 STOCK SUMMARY (STATS)
        // =========================================================
        [HttpGet("stats")]
        public async Task<IActionResult> GetStockStats(string status = "In Stock")
        {
            var parts = await _context.Parts
                .Include(p => p.StockMovements)
                .Where(p => p.IsActive)
                .ToListAsync();

            // Filter by stock status
            if (status == "In Stock")
                parts = parts.Where(p => p.StockMovements.Sum(m => m.Quantity) > 0).ToList();
            else if (status == "Out of Stock")
                parts = parts.Where(p => p.StockMovements.Sum(m => m.Quantity) <= 0).ToList();

            var stats = new StockStatusDto
            {
                UniquePartNos = parts.Count,
                TotalStockItems = parts.Sum(p => p.StockMovements.Sum(m => m.Quantity)),
                StockValue = parts.Sum(p => p.StockMovements.Where(m => m.Quantity > 0).Sum(m => m.Quantity * m.PurchasePrice))
            };

            return Ok(stats);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchParts([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new List<IdNameDto>());

            var parts = await _autoCompleteRepository.SearchParts(query);
            return Ok(parts);
        }
        [HttpGet("get/{id}")]
        public async Task<ActionResult<PartDto>> GetPartById(long id)
        {
            var part = await _context.Parts
                .Where(p => p.Id == id)
                .Select(p => new PartDto
                {
                    Id = p.Id,
                    PartNo = p.PartNo,
                    PartName = p.PartName,
                    Brand = p.Brand,
                    SellingPrice = p.StockMovements.Sum(sm => sm.Quantity > 0 ? sm.SellingPrice : 0), // or latest price
                    QtyOnHand = p.StockMovements.Sum(sm => sm.Quantity)
                })
                .FirstOrDefaultAsync();

            if (part == null) return NotFound();
            return Ok(part);
        }

        public class PartDto
        {
            public long Id { get; set; }
            public string PartNo { get; set; } = string.Empty;
            public string PartName { get; set; } = string.Empty;
            public string Brand { get; set; } = string.Empty;
            public decimal SellingPrice { get; set; }
            public decimal QtyOnHand { get; set; }
        }

    }
}
