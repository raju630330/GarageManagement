using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GarageManagement.Server.Controllers
{
    [Route("api/stock")]
    public class StockController : BaseAuthorizationController
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // 🔹 STOCK LIST
        // =========================================================
        [HttpGet("list")]
        public async Task<IActionResult> GetStockList(string stockType = "IN", string search = "")
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
                stockType == "OUT" ? x.QtyOnHand <= 0 : true)
            .OrderBy(x => x.PartName)
            .ToList();

            return Ok(stockList);
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
    }
}
