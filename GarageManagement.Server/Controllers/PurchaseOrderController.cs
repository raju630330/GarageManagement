using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using GarageManagement.Server.RepoInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GarageManagement.Server.Data;

namespace GarageManagement.Server.Controllers
{
    [Authorize]
    public class PurchaseOrderController : BaseAuthorizationController
    {
        private readonly IPurchaseOrderRepository _orderRepo;
        private readonly IPurchaseOrderItemRepository _itemRepo;
        private readonly IStockMovementRepository _stockRepo;
        private readonly ISupplierRepository _supplierRepo;
        private readonly ApplicationDbContext _db;

        public PurchaseOrderController(
            IPurchaseOrderRepository orderRepo,
            IPurchaseOrderItemRepository itemRepo,
            IStockMovementRepository stockRepo,
            ISupplierRepository supplierRepo,
            ApplicationDbContext db)
        {
            _orderRepo = orderRepo;
            _itemRepo = itemRepo;
            _stockRepo = stockRepo;
            _supplierRepo = supplierRepo;
            _db = db;
        }

        // ── POST /api/purchaseorder ──────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new CreateOrderResponse { IsSuccess = false, Message = "Invalid request data" });

            var workshopId = GetWorkshopId();
            var userId = GetUserId();

            if (!await _supplierRepo.ExistsAsync(dto.SupplierId, workshopId))
                return BadRequest(new CreateOrderResponse { IsSuccess = false, Message = "Supplier not found or inactive" });

            if (dto.PaymentType != "Cash" && dto.PaymentType != "Credit")
                return BadRequest(new CreateOrderResponse { IsSuccess = false, Message = "PaymentType must be Cash or Credit" });

            var partIds = dto.Items.Select(i => i.PartId).Distinct().ToList();
            var validParts = await _db.Parts
                .Where(p => partIds.Contains(p.Id) && p.WorkshopId == workshopId && p.IsActive)
                .ToDictionaryAsync(p => p.Id);

            if (validParts.Count != partIds.Count)
                return BadRequest(new CreateOrderResponse { IsSuccess = false, Message = "One or more parts are invalid or inactive" });

            foreach (var item in dto.Items)
            {
                if (item.Discount > item.Qty * item.UnitPrice)
                    return BadRequest(new CreateOrderResponse
                    {
                        IsSuccess = false,
                        Message = $"Discount on partId {item.PartId} exceeds item amount"
                    });
            }

            if (dto.JobCardId.HasValue)
            {
                var jcExists = await _db.JobCards
                    .AnyAsync(j => j.Id == dto.JobCardId.Value && j.WorkshopId == workshopId);
                if (!jcExists)
                    return BadRequest(new CreateOrderResponse { IsSuccess = false, Message = "Job card not found" });
            }

            var latestNo = await _orderRepo.GetLatestOrderNoAsync(workshopId);
            var orderNo = GenerateOrderNo(latestNo, workshopId);
            var now = DateTime.UtcNow;

            var orderId = await _orderRepo.CreateAsync(new PurchaseOrder
            {
                OrderNo = orderNo,
                SupplierId = dto.SupplierId,
                PaymentType = dto.PaymentType,
                StockType = dto.StockType ?? string.Empty,
                Remarks = dto.Remarks ?? string.Empty,
                JobCardId = dto.JobCardId,
                RegNo = string.IsNullOrWhiteSpace(dto.RegNo) ? "STOCK" : dto.RegNo,
                JobCardNo = dto.JobCardNo ?? string.Empty,
                Source = dto.Source ?? string.Empty,
                OrderDate = now,
                Status = "PENDING",
                WorkshopId = workshopId,
                RowState = 1,
                ModifiedBy = userId,
                ModifiedOn = now
            });

            var items = new List<PurchaseOrderItem>();
            var movements = new List<StockMovement>();

            foreach (var item in dto.Items)
            {
                var part = validParts[item.PartId];
                var baseAmt = item.Qty * item.UnitPrice - item.Discount;
                var taxAmt = Math.Round(baseAmt * part.TaxPercent / 100, 2);
                var total = Math.Round(baseAmt + taxAmt, 2);

                items.Add(new PurchaseOrderItem
                {
                    PurchaseOrderId = orderId,
                    PartId = item.PartId,
                    Qty = item.Qty,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount,
                    TaxPercent = part.TaxPercent,
                    TaxAmount = taxAmt,
                    TotalPurchasePrice = total,
                    ServiceType = item.ServiceType ?? "Part",
                    Remarks = item.Remarks ?? string.Empty,
                    SellerInfo = item.SellerInfo ?? string.Empty,
                    InwardedQty = 0,
                    WorkshopId = workshopId,
                    RowState = 1,
                    ModifiedBy = userId,
                    ModifiedOn = now
                });

                movements.Add(new StockMovement
                {
                    PartId = item.PartId,
                    Quantity = item.Qty,
                    PurchasePrice = item.UnitPrice,
                    SellingPrice = 0,
                    TransactionType = "ORDER",
                    TransactionDate = now,
                    Barcode = string.Empty,
                    UserId = userId,
                    WorkshopId = workshopId,
                    RowState = 1,
                    ModifiedBy = userId,
                    ModifiedOn = now
                });
            }

            await _itemRepo.AddRangeAsync(items);
            await _stockRepo.AddRangeAsync(movements);

            return Ok(new CreateOrderResponse { IsSuccess = true, OrderNo = orderNo, Message = "Order created successfully" });
        }

        // ── GET /api/purchaseorder ───────────────────────────────────────────
        // Angular sends: search, status, fromDate, toDate, page, pageSize
        // These are bound individually because OrderFilterDto only has
        // Status, RegNo, From (DateTime?), To (DateTime?) — no Search/Page/PageSize.
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] string search = "",
            [FromQuery] string status = "All",
            [FromQuery] string fromDate = "",
            [FromQuery] string toDate = "",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var workshopId = GetWorkshopId();

            // Build the repo's OrderFilterDto using the fields it actually has.
            // For status: repo filters exact match — pass empty string for "All"
            //             so the repo WHERE clause is skipped (repo checks IsNullOrWhiteSpace).
            // For search: repo only supports RegNo filter, so pass search there;
            //             we do a broader in-memory filter on OrderNo + SupplierName below.
            var repoFilter = new OrderFilterDto
            {
                Status = status == "All" ? null : status,
                RegNo = string.IsNullOrWhiteSpace(search) ? null : search,
                From = DateTime.TryParse(fromDate, out var f) ? f : (DateTime?)null,
                To = DateTime.TryParse(toDate, out var t) ? t : (DateTime?)null,
            };

            // Repo call — returns IEnumerable<OrderListDto>
            var all = (await _orderRepo.GetAllAsync(workshopId, repoFilter)).ToList();

            // Broaden the search in-memory to also cover OrderNo and SupplierName
            // (repo only filtered on RegNo above)
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                all = all.Where(o =>
                    (o.OrderNo ?? "").ToLower().Contains(term) ||
                    (o.SupplierName ?? "").ToLower().Contains(term) ||
                    (o.RegNo ?? "").ToLower().Contains(term)
                ).ToList();
            }

            // Pagination
            var totalRecords = all.Count;
            var safePage = page > 0 ? page : 1;
            var safePageSize = pageSize > 0 ? pageSize : 10;
            var paged = all.Skip((safePage - 1) * safePageSize).Take(safePageSize).ToList();

            // Map OrderListDto → camelCase shape Angular expects.
            // OrderListDto has: Id, OrderNo, OrderDate (string "dd-MM-yyyy"),
            //   SupplierName, RegNo, JobCardNo, OrderValue, OrderedParts,
            //   InwardedParts, PendingParts, Status
            var items = paged.Select(o => new
            {
                orderId = o.Id,
                orderNo = o.OrderNo,
                // Re-parse "dd-MM-yyyy" → ISO so Angular's date pipe works
                orderDate = DateTime.TryParseExact(
                                   o.OrderDate, "dd-MM-yyyy",
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   System.Globalization.DateTimeStyles.None, out var d)
                               ? d.ToString("yyyy-MM-ddTHH:mm:ss")
                               : o.OrderDate,
                supplierName = o.SupplierName,
                regNo = o.RegNo,
                jobCardNo = o.JobCardNo,
                paymentType = o.PaymentType,          
                stockType = o.StockType,           
                status = o.Status,
                totalAmount = o.OrderValue,
                itemCount = o.OrderedParts,
                orderedParts = o.OrderedParts,
                inwardedParts = o.InwardedParts,
                pendingParts = o.PendingParts
            }).ToList();

            return Ok(new { totalRecords, page = safePage, pageSize = safePageSize, items });
        }

        // ── GET /api/purchaseorder/{id} ──────────────────────────────────────
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetOrder(long id)
        {
            var workshopId = GetWorkshopId();
            var order = await _orderRepo.GetByIdAsync(id, workshopId);

            if (order == null)
                return NotFound(new { message = "Order not found" });

            // GetByIdAsync already .Include(o => o.Items) so Items is populated.
            // Parts are not included via navigation — load them separately.
            var orderItems = order.Items ?? new List<PurchaseOrderItem>();
            var partIds = orderItems.Select(i => i.PartId).Distinct().ToList();

            var parts = partIds.Count > 0
                ? await _db.Parts
                    .Where(p => partIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id)
                : new Dictionary<long, Part>();

            var dto = new
            {
                orderId = order.Id,
                orderNo = order.OrderNo,
                orderDate = order.OrderDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                supplierName = order.Supplier?.Name ?? "",
                supplierPhone = order.Supplier?.Phone ?? "",
                regNo = order.RegNo,
                jobCardNo = order.JobCardNo,
                paymentType = order.PaymentType,
                stockType = order.StockType,
                remarks = order.Remarks,
                status = order.Status,
                items = orderItems.Select(i =>
                {
                    parts.TryGetValue(i.PartId, out var part);
                    return new
                    {
                        partId = i.PartId,
                        partName = part?.PartName ?? "",
                        partNo = part?.PartNo ?? "",
                        brand = part?.Brand ?? "",
                        hsnCode = "",
                        taxPercent = i.TaxPercent,
                        qty = i.Qty,
                        unitPrice = i.UnitPrice,
                        discount = i.Discount,
                        taxAmount = i.TaxAmount,
                        totalPurchasePrice = i.TotalPurchasePrice,
                        serviceType = i.ServiceType,
                        remarks = i.Remarks ?? "",
                        sellerInfo = i.SellerInfo ?? "",
                        inwardedQty = i.InwardedQty
                    };
                }).ToList()
            };

            return Ok(dto);
        }

        // ── PUT /api/purchaseorder/{id}/status ───────────────────────────────
        [HttpPut("{id:long}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateStatusDto dto)
        {
            var allowed = new[] { "PENDING", "SHIPMENT", "CLOSED", "CANCELLED" };
            if (!allowed.Contains(dto.Status))
                return BadRequest(new { message = "Invalid status value" });

            var order = await _orderRepo.GetByIdAsync(id, GetWorkshopId());
            if (order == null)
                return NotFound(new { message = "Order not found" });

            if (order.Status == "CLOSED")
                return BadRequest(new { message = "Cannot change status of a closed order" });

            await _orderRepo.UpdateStatusAsync(id, dto.Status, GetUserId());
            return Ok(new { isSuccess = true, message = $"Status updated to {dto.Status}" });
        }

        // ── DELETE /api/purchaseorder/{id} ───────────────────────────────────
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> CancelOrder(long id)
        {
            var order = await _orderRepo.GetByIdAsync(id, GetWorkshopId());
            if (order == null)
                return NotFound(new { message = "Order not found" });

            if (order.Status == "CLOSED")
                return BadRequest(new { message = "Cannot cancel a closed order" });

            await _orderRepo.SoftDeleteAsync(id, GetUserId());
            return Ok(new { isSuccess = true, message = "Order cancelled" });
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private static string GenerateOrderNo(string? latest, long workshopId)
        {
            int next = 1;
            if (latest != null)
            {
                var idx = latest.IndexOf("STOR");
                if (idx >= 0 && int.TryParse(latest.Substring(idx + 4), out int parsed))
                    next = parsed + 1;
            }
            return $"SRT-STOR{next:D6}";
        }

        private long GetWorkshopId() =>
            long.Parse(User.FindFirst("WorkshopId")?.Value ?? "0");

        private long GetUserId() =>
            long.Parse(User.FindFirst("UserId")?.Value ?? "0");
    }
}