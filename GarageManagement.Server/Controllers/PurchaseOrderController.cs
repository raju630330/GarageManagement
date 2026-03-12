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

        // Only for Part + JobCard validation (no separate Part repo yet)
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
        // Called by Angular order.component.ts → submitOrder()
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new CreateOrderResponse { IsSuccess = false, Message = "Invalid request data" });

            var workshopId = GetWorkshopId();
            var userId = GetUserId();

            // 1. Validate Supplier
            if (!await _supplierRepo.ExistsAsync(dto.SupplierId, workshopId))
                return BadRequest(new CreateOrderResponse { IsSuccess = false, Message = "Supplier not found or inactive" });

            // 2. Validate PaymentType
            if (dto.PaymentType != "Cash" && dto.PaymentType != "Credit")
                return BadRequest(new CreateOrderResponse { IsSuccess = false, Message = "PaymentType must be Cash or Credit" });

            // 3. Validate all Part IDs in one DB call
            var partIds = dto.Items.Select(i => i.PartId).Distinct().ToList();
            var validParts = await _db.Parts
                .Where(p => partIds.Contains(p.Id) && p.WorkshopId == workshopId && p.IsActive)
                .ToDictionaryAsync(p => p.Id);

            if (validParts.Count != partIds.Count)
                return BadRequest(new CreateOrderResponse { IsSuccess = false, Message = "One or more parts are invalid or inactive" });

            // 4. Item-level discount rule
            foreach (var item in dto.Items)
            {
                if (item.Discount > item.Qty * item.UnitPrice)
                    return BadRequest(new CreateOrderResponse
                    {
                        IsSuccess = false,
                        Message = $"Discount on partId {item.PartId} exceeds item amount"
                    });
            }

            // 5. Validate JobCard if provided
            if (dto.JobCardId.HasValue)
            {
                var jcExists = await _db.JobCards
                    .AnyAsync(j => j.Id == dto.JobCardId.Value && j.WorkshopId == workshopId);

                if (!jcExists)
                    return BadRequest(new CreateOrderResponse { IsSuccess = false, Message = "Job card not found" });
            }

            // 6. Generate Order Number
            var latestNo = await _orderRepo.GetLatestOrderNoAsync(workshopId);
            var orderNo = GenerateOrderNo(latestNo, workshopId);
            var now = DateTime.UtcNow;

            // 7. Create PurchaseOrder
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

            // 8. Build Items + StockMovements
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

            // 9. Bulk insert items and stock movements
            await _itemRepo.AddRangeAsync(items);
            await _stockRepo.AddRangeAsync(movements);

            return Ok(new CreateOrderResponse
            {
                IsSuccess = true,
                OrderNo = orderNo,
                Message = "Order created successfully"
            });
        }

        // ── GET /api/purchaseorder ───────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] OrderFilterDto filter)
        {
            var orders = await _orderRepo.GetAllAsync(GetWorkshopId(), filter);
            return Ok(orders);
        }

        // ── GET /api/purchaseorder/{id} ──────────────────────────────────────
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetOrder(long id)
        {
            var order = await _orderRepo.GetByIdAsync(id, GetWorkshopId());
            return order == null
                ? NotFound(new { message = "Order not found" })
                : Ok(order);
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
            // Prefix: first 3 letters of workshopId or hardcoded — adjust to match your pattern
            return $"SRT-STOR{next:D6}";
        }

        // Replace with your actual JWT claim helpers
        private long GetWorkshopId() =>
            long.Parse(User.FindFirst("WorkshopId")?.Value ?? "0");

        private long GetUserId() =>
            long.Parse(User.FindFirst("UserId")?.Value ?? "0");
    }
}