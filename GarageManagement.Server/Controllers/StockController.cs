using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.RepoInterfaces;
using GarageManagement.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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
        // FIX 3: Added workshopId filter to every endpoint
        // FIX 4: Search filter pushed into EF query (SQL-level)
        //        instead of loading all parts into memory first
        // =========================================================
        private long GetWorkshopId() =>
            long.Parse(User.FindFirst("WorkshopId")?.Value ?? "0");

        // =========================================================
        // STOCK LIST
        // =========================================================
        [HttpGet("list")]
        public async Task<IActionResult> GetStockList(
            string stockType = "IN",
            string search = "",
            int page = 1,
            int pageSize = 10)
        {
            var workshopId = GetWorkshopId();   // FIX 3

            // FIX 4: filter at SQL level — do NOT ToListAsync() before search
            var query = _context.Parts
                .Include(p => p.StockMovements)
                .Where(p => p.IsActive && p.WorkshopId == workshopId);   // FIX 3

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.PartNo.Contains(search) ||
                    p.PartName.Contains(search) ||
                    p.Brand.Contains(search));
            }

            var parts = await query.ToListAsync();

            // Map to DTO and calculate quantities and averages
            var stockList = parts.Select(p =>
            {
                // FIX 2 (applied here too): only INWARD movements for purchase price avg
                var inwardMovements = p.StockMovements
                    .Where(m => m.TransactionType == "INWARD" && m.Quantity > 0)
                    .ToList();

                var qtyOnHand = p.StockMovements.Sum(m => m.Quantity);

                return new StockListDto
                {
                    PartNo = p.PartNo,
                    PartName = p.PartName,
                    Brand = p.Brand,
                    Category = p.Category,
                    QtyOnHand = qtyOnHand,
                    AvgPurchasePrice = inwardMovements.Any()
                                        ? inwardMovements.Average(m => m.PurchasePrice)
                                        : 0,
                    AvgSellingPrice = inwardMovements.Any()
                                        ? inwardMovements.Average(m => m.SellingPrice)
                                        : 0,
                    TaxType = p.TaxType,
                    TaxPercent = p.TaxPercent,
                    TaxAmount = (inwardMovements.Any()
                                        ? inwardMovements.Average(m => m.SellingPrice)
                                        : 0) * p.TaxPercent / 100,
                    RackNo = p.RackNo,
                    Ageing = inwardMovements.Any()
                                        ? inwardMovements.Min(m => (int)(DateTime.Now - m.TransactionDate).TotalDays)
                                        : 0,
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

            var totalRecords = stockList.Count;

            var pagedData = stockList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new { totalRecords, page, pageSize, items = pagedData });
        }

        // =========================================================
        // STOCK SUMMARY (STATS)
        // =========================================================
        [HttpGet("stats")]
        public async Task<IActionResult> GetStockStats(string status = "In Stock")
        {
            var workshopId = GetWorkshopId();   // FIX 3

            // FIX 4: filter at SQL level
            var parts = await _context.Parts
                .Include(p => p.StockMovements)
                .Where(p => p.IsActive && p.WorkshopId == workshopId)   // FIX 3
                .ToListAsync();

            if (status == "In Stock")
                parts = parts.Where(p => p.StockMovements.Sum(m => m.Quantity) > 0).ToList();
            else if (status == "Out of Stock")
                parts = parts.Where(p => p.StockMovements.Sum(m => m.Quantity) <= 0).ToList();

            var stats = new StockStatusDto
            {
                UniquePartNos = parts.Count,
                TotalStockItems = parts.Sum(p => p.StockMovements.Sum(m => m.Quantity)),
                StockValue = parts.Sum(p =>
                    p.StockMovements
                        .Where(m => m.TransactionType == "INWARD" && m.Quantity > 0)
                        .Sum(m => m.Quantity * m.PurchasePrice))
            };

            return Ok(stats);
        }

        // =========================================================
        // AUTOCOMPLETE SEARCH
        // =========================================================
        [HttpGet("search")]
        public async Task<IActionResult> SearchParts([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new List<IdNameDto>());

            // FIX 3: pass workshopId so autocomplete only returns this workshop's parts
            var workshopId = GetWorkshopId();
            var parts = await _autoCompleteRepository.SearchParts(query, workshopId);
            return Ok(parts);
        }

        // =========================================================
        // GET SINGLE PART BY ID
        // =========================================================
        [HttpGet("get/{id}")]
        public async Task<ActionResult<PartDto>> GetPartById(long id)
        {
            var workshopId = GetWorkshopId();   // FIX 3

            var part = await _context.Parts
                .Where(p => p.Id == id && p.WorkshopId == workshopId)   // FIX 3
                .Select(p => new PartDto
                {
                    Id = p.Id,
                    PartNo = p.PartNo,
                    PartName = p.PartName,
                    Brand = p.Brand,
                    TaxPercent = p.TaxPercent,
                    HsnCode = "",

                    // FIX 2: Only INWARD movements have a real SellingPrice.
                    // ORDER movements save SellingPrice = 0 (unknown at order time).
                    // Without this filter, a recently-ordered part returns 0
                    // to the Estimation module.
                    SellingPrice = p.StockMovements
                        .Where(sm => sm.TransactionType == "INWARD" && sm.Quantity > 0)
                        .OrderByDescending(sm => sm.TransactionDate)
                        .Select(sm => sm.SellingPrice)
                        .FirstOrDefault(),

                    // Order module pre-fills purchase price from latest inward
                    PurchasePrice = p.StockMovements
                        .Where(sm => sm.TransactionType == "INWARD" && sm.Quantity > 0)
                        .OrderByDescending(sm => sm.TransactionDate)
                        .Select(sm => sm.PurchasePrice)
                        .FirstOrDefault(),

                    QtyOnHand = p.StockMovements.Sum(sm => sm.Quantity)
                })
                .FirstOrDefaultAsync();

            if (part == null) return NotFound();
            return Ok(part);
        }

        // =========================================================
        // PartDto — JSON property names match Angular expectations
        // =========================================================
        public class PartDto
        {
            [JsonPropertyName("partId")]
            public long Id { get; set; }

            [JsonPropertyName("partNo")]
            public string PartNo { get; set; } = string.Empty;

            [JsonPropertyName("partName")]
            public string PartName { get; set; } = string.Empty;

            [JsonPropertyName("brand")]
            public string Brand { get; set; } = string.Empty;

            [JsonPropertyName("taxPercent")]
            public decimal TaxPercent { get; set; }

            [JsonPropertyName("hsnCode")]
            public string HsnCode { get; set; } = string.Empty;

            // Estimation module reads this → charge to customer
            [JsonPropertyName("sellingPrice")]
            public decimal SellingPrice { get; set; }

            // Order + Inward modules read this → pay to supplier
            [JsonPropertyName("purchasePrice")]
            public decimal PurchasePrice { get; set; }

            [JsonPropertyName("qtyOnHand")]
            public decimal QtyOnHand { get; set; }
        }
    }
}