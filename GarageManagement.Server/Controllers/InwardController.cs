
using GarageManagement.Server.Data;
using GarageManagement.Server.dtos;
using GarageManagement.Server.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageManagement.Server.Controllers
{
    public class InwardController : BaseAuthorizationController
    {
        private readonly ApplicationDbContext _db;

        public InwardController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ── POST /api/inward ───────────────────────────────────────────────
        // Called by Angular inward.component.ts → submitInward()
        // Business logic (document pages 46-48, 65-67):
        //   1. Validate supplier + bill + parts
        //   2. INSERT InwardNote (header)
        //   3. For each item:
        //      a. INSERT InwardNoteItem
        //      b. INSERT StockMovement (TransactionType = "INWARD", Qty = +inwardQty)
        //      c. UPDATE Part.RackNo if provided
        //      d. If PurchaseOrderItemId provided:
        //         – UPDATE PurchaseOrderItem.InwardedQty += inwardQty
        //         – UPDATE PurchaseOrderItem.StockMovementId = new StockMovement.Id
        //   4. If all items in the PO are fully inwarded → UPDATE PO.Status = "CLOSED"
        //      If partial → UPDATE PO.Status = "SHIPMENT"
        //   5. Return { isSuccess, inwardNo }
        [HttpPost]
        public async Task<IActionResult> CreateInward([FromBody] CreateInwardDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new CreateInwardResponse { IsSuccess = false, Message = "Invalid request data" });

            var workshopId = GetWorkshopId();
            var userId = GetUserId();

            // ── 1a. Validate Supplier ──────────────────────────────────────
            var supplierExists = await _db.Suppliers
                .AnyAsync(s => s.Id == dto.SupplierId && s.WorkshopId == workshopId && s.IsActive);

            if (!supplierExists)
                return BadRequest(new CreateInwardResponse { IsSuccess = false, Message = "Supplier not found or inactive" });

            // ── 1b. Validate all Part IDs ──────────────────────────────────
            var partIds = dto.Items.Select(i => i.PartId).Distinct().ToList();
            var validParts = await _db.Parts
                .Where(p => partIds.Contains(p.Id) && p.WorkshopId == workshopId && p.IsActive)
                .ToDictionaryAsync(p => p.Id);

            if (validParts.Count != partIds.Count)
                return BadRequest(new CreateInwardResponse { IsSuccess = false, Message = "One or more parts are invalid or inactive" });

            // ── 1c. Item-level validation ──────────────────────────────────
            foreach (var item in dto.Items)
            {
                if (item.Discount > item.InwardQty * item.UnitPrice)
                    return BadRequest(new CreateInwardResponse
                    {
                        IsSuccess = false,
                        Message = $"Discount on partId {item.PartId} exceeds item amount"
                    });

                if (item.SellingPrice <= 0)
                    return BadRequest(new CreateInwardResponse
                    {
                        IsSuccess = false,
                        Message = $"Selling price for partId {item.PartId} must be greater than 0"
                    });
            }

            // ── 1d. Validate JobCard if provided ───────────────────────────
            if (dto.JobCardId.HasValue)
            {
                var jcExists = await _db.JobCards
                    .AnyAsync(j => j.Id == dto.JobCardId.Value && j.WorkshopId == workshopId);
                if (!jcExists)
                    return BadRequest(new CreateInwardResponse { IsSuccess = false, Message = "Job card not found" });
            }

            // ── 2. Generate Inward Number ──────────────────────────────────
            // Pattern: SRT-JCIN000829
            var latestInwardNo = await _db.InwardNotes
                .Where(n => n.WorkshopId == workshopId)
                .OrderByDescending(n => n.Id)
                .Select(n => n.InwardNo)
                .FirstOrDefaultAsync();

            var inwardNo = GenerateInwardNo(latestInwardNo);
            var now = DateTime.UtcNow;

            // ── 3. INSERT InwardNote (header) ──────────────────────────────
            var inwardNote = new InwardNote
            {
                InwardNo = inwardNo,
                PurchaseOrderId = dto.OrderId,
                JobCardId = dto.JobCardId,
                RegNo = string.IsNullOrWhiteSpace(dto.RegNo) ? "STOCK" : dto.RegNo,
                JobCardNo = dto.JobCardNo ?? string.Empty,
                DeliveryReceipt = dto.DeliveryReceipt ?? string.Empty,
                BillNo = dto.BillNo,
                BillDate = dto.BillDate,
                TaxType = dto.TaxType,
                SupplierId = dto.SupplierId,
                FreightAmount = dto.FreightAmount,
                TcsAmount = dto.TcsAmount,
                PaidAmount = dto.PaidAmount,
                WorkshopId = workshopId,
                RowState = 1,
                ModifiedBy = userId,
                ModifiedOn = now
            };

            _db.InwardNotes.Add(inwardNote);
            await _db.SaveChangesAsync();   // need InwardNote.Id before inserting items

            // ── 4. Process each item ───────────────────────────────────────
            foreach (var itemDto in dto.Items)
            {
                var part = validParts[itemDto.PartId];
                var taxPct = itemDto.TaxPercent > 0 ? itemDto.TaxPercent : part.TaxPercent;
                var baseAmt = itemDto.InwardQty * itemDto.UnitPrice - itemDto.Discount;
                var taxAmt = Math.Round(baseAmt * taxPct / 100, 2);
                var total = Math.Round(baseAmt + taxAmt, 2);
                var margin = Math.Round(itemDto.SellingPrice - itemDto.UnitPrice, 2);

                // 4a. INSERT InwardNoteItem
                var inwardItem = new InwardNoteItem
                {
                    InwardNoteId = inwardNote.Id,
                    PartId = itemDto.PartId,
                    PurchaseOrderItemId = itemDto.PurchaseOrderItemId,
                    InwardQty = itemDto.InwardQty,
                    UnitPrice = itemDto.UnitPrice,
                    Discount = itemDto.Discount,
                    TaxPercent = taxPct,
                    TaxAmount = taxAmt,
                    TotalPurchasePrice = total,
                    SellingPrice = itemDto.SellingPrice,
                    Margin = margin,
                    RackNo = itemDto.RackNo ?? string.Empty,
                    Barcode = itemDto.Barcode ?? string.Empty,
                    Remarks = itemDto.Remarks ?? string.Empty,
                    WorkshopId = workshopId,
                    RowState = 1,
                    ModifiedBy = userId,
                    ModifiedOn = now
                };
                _db.InwardNoteItems.Add(inwardItem);

                // 4b. INSERT StockMovement (INWARD increases physical stock)
                var stockMovement = new StockMovement
                {
                    PartId = itemDto.PartId,
                    Quantity = itemDto.InwardQty,    // positive = stock in
                    PurchasePrice = itemDto.UnitPrice,
                    SellingPrice = itemDto.SellingPrice,
                    TransactionType = "INWARD",
                    TransactionDate = now,
                    Barcode = itemDto.Barcode ?? string.Empty,
                    UserId = userId,
                    WorkshopId = workshopId,
                    RowState = 1,
                    ModifiedBy = userId,
                    ModifiedOn = now
                };
                _db.StockMovements.Add(stockMovement);
                await _db.SaveChangesAsync();   // need StockMovement.Id for linking

                // 4c. UPDATE Part.RackNo if provided
                if (!string.IsNullOrWhiteSpace(itemDto.RackNo))
                {
                    part.RackNo = itemDto.RackNo;
                }

                // 4d. UPDATE Part.SellingPrice to latest inward selling price
                // (optional — keeps stock selling price fresh)
                // part.SellingPrice = itemDto.SellingPrice;

                // 4e. If linked to a PurchaseOrderItem, update inwarded qty + stockMovementId
                if (itemDto.PurchaseOrderItemId.HasValue)
                {
                    var poItem = await _db.PurchaseOrderItems
                        .FirstOrDefaultAsync(i => i.Id == itemDto.PurchaseOrderItemId.Value
                                               && i.WorkshopId == workshopId);
                    if (poItem != null)
                    {
                        poItem.InwardedQty += itemDto.InwardQty;
                        poItem.StockMovementId = stockMovement.Id;
                        poItem.ModifiedBy = userId;
                        poItem.ModifiedOn = now;
                    }
                }
            }

            await _db.SaveChangesAsync();

            // ── 5. Update PurchaseOrder status if linked ───────────────────
            if (dto.OrderId.HasValue)
            {
                var order = await _db.PurchaseOrders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.Id == dto.OrderId.Value
                                           && o.WorkshopId == workshopId);

                if (order != null)
                {
                    var allClosed = order.Items
                        .Where(i => i.RowState == 1)
                        .All(i => i.InwardedQty >= i.Qty);

                    order.Status = allClosed ? "CLOSED" : "SHIPMENT";
                    order.ModifiedBy = userId;
                    order.ModifiedOn = now;
                    await _db.SaveChangesAsync();
                }
            }

            return Ok(new CreateInwardResponse
            {
                IsSuccess = true,
                InwardNo = inwardNo,
                Message = "Inward created successfully"
            });
        }

        // ── GET /api/inward ────────────────────────────────────────────────
        // Powers the Inward list tab (document pages 54-56)
        // Columns: #, Job Card No, Reg No, Inward No, Order No,
        //          Vendor Name, Order Date, Inward Date, Bill No / Receipt No
        [HttpGet]
        public async Task<IActionResult> GetInwards(
            [FromQuery] string search = "",
            [FromQuery] string fromDate = "",
            [FromQuery] string toDate = "",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var workshopId = GetWorkshopId();

            var query = _db.InwardNotes
                .Include(n => n.Supplier)
                .Where(n => n.WorkshopId == workshopId && n.RowState == 1)
                .AsQueryable();

            // Date filters
            if (DateTime.TryParse(fromDate, out var from))
                query = query.Where(n => n.BillDate >= from);

            if (DateTime.TryParse(toDate, out var to))
                query = query.Where(n => n.BillDate <= to.AddDays(1));

            var all = await query
                .OrderByDescending(n => n.Id)
                .Select(n => new InwardListDto
                {
                    Id = n.Id,
                    InwardNo = n.InwardNo,
                    InwardDate = n.ModifiedOn.ToString("dd-MM-yyyy"),
                    OrderNo = n.PurchaseOrderId.HasValue
                                   ? _db.PurchaseOrders
                                         .Where(o => o.Id == n.PurchaseOrderId.Value)
                                         .Select(o => o.OrderNo)
                                         .FirstOrDefault() ?? ""
                                   : "",
                    JobCardNo = n.JobCardNo,
                    RegNo = n.RegNo,
                    VendorName = n.Supplier != null ? n.Supplier.Name : "",
                    BillNo = n.BillNo,
                    InwardValue = _db.InwardNoteItems
                                      .Where(i => i.InwardNoteId == n.Id && i.RowState == 1)
                                      .Sum(i => i.TotalPurchasePrice)
                })
                .ToListAsync();

            // In-memory search on InwardNo, RegNo, VendorName, BillNo
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                all = all.Where(n =>
                    (n.InwardNo ?? "").ToLower().Contains(term) ||
                    (n.RegNo ?? "").ToLower().Contains(term) ||
                    (n.VendorName ?? "").ToLower().Contains(term) ||
                    (n.BillNo ?? "").ToLower().Contains(term) ||
                    (n.JobCardNo ?? "").ToLower().Contains(term)
                ).ToList();
            }

            var totalRecords = all.Count;
            var safePage = page > 0 ? page : 1;
            var safeSize = pageSize > 0 ? pageSize : 10;
            var items = all.Skip((safePage - 1) * safeSize).Take(safeSize).ToList();

            return Ok(new { totalRecords, page = safePage, pageSize = safeSize, items });
        }

        // ── GET /api/inward/{id} ───────────────────────────────────────────
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetInward(long id)
        {
            var workshopId = GetWorkshopId();

            var note = await _db.InwardNotes
                .Include(n => n.Supplier)
                .Include(n => n.Items)
                .FirstOrDefaultAsync(n => n.Id == id
                                       && n.WorkshopId == workshopId
                                       && n.RowState == 1);

            if (note == null)
                return NotFound(new { message = "Inward note not found" });

            // Enrich items with part details
            var partIds = note.Items.Select(i => i.PartId).Distinct().ToList();
            var parts = partIds.Count > 0
                ? await _db.Parts
                    .Where(p => partIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id)
                : new Dictionary<long, Part>();

            var dto = new
            {
                inwardId = note.Id,
                inwardNo = note.InwardNo,
                inwardDate = note.ModifiedOn.ToString("yyyy-MM-ddTHH:mm:ss"),
                orderId = note.PurchaseOrderId,
                jobCardId = note.JobCardId,
                regNo = note.RegNo,
                jobCardNo = note.JobCardNo,
                deliveryReceipt = note.DeliveryReceipt,
                billNo = note.BillNo,
                billDate = note.BillDate.ToString("yyyy-MM-dd"),
                taxType = note.TaxType,
                supplierId = note.SupplierId,
                supplierName = note.Supplier?.Name ?? "",
                freightAmount = note.FreightAmount,
                tcsAmount = note.TcsAmount,
                paidAmount = note.PaidAmount,
                items = note.Items.Select(i =>
                {
                    parts.TryGetValue(i.PartId, out var part);
                    return new
                    {
                        partId = i.PartId,
                        partName = part?.PartName ?? "",
                        partNo = part?.PartNo ?? "",
                        brand = part?.Brand ?? "",
                        inwardQty = i.InwardQty,
                        unitPrice = i.UnitPrice,
                        discount = i.Discount,
                        taxPercent = i.TaxPercent,
                        taxAmount = i.TaxAmount,
                        totalPurchasePrice = i.TotalPurchasePrice,
                        sellingPrice = i.SellingPrice,
                        margin = i.Margin,
                        rackNo = i.RackNo,
                        barcode = i.Barcode,
                        remarks = i.Remarks
                    };
                }).ToList()
            };

            return Ok(dto);
        }

        // ── GET /api/inward/pending-orders ────────────────────────────────
        // Returns Purchase Orders that still have pending (un-inwarded) items.
        // Used in dropdown when user opens Inward from Stock module.
        [HttpGet("pending-orders")]
        public async Task<IActionResult> GetPendingOrders()
        {
            var workshopId = GetWorkshopId();

            var pendingOrders = await _db.PurchaseOrders
                .Include(o => o.Supplier)
                .Include(o => o.Items)
                .Where(o => o.WorkshopId == workshopId
                         && o.RowState == 1
                         && o.Status != "CLOSED"
                         && o.Status != "CANCELLED"
                         && o.Items.Any(i => i.InwardedQty < i.Qty && i.RowState == 1))
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new
                {
                    orderId = o.Id,
                    orderNo = o.OrderNo,
                    orderDate = o.OrderDate.ToString("dd-MM-yyyy"),
                    supplierName = o.Supplier != null ? o.Supplier.Name : "",
                    regNo = o.RegNo,
                    jobCardNo = o.JobCardNo,
                    pendingParts = o.Items.Count(i => i.InwardedQty < i.Qty && i.RowState == 1),
                    status = o.Status
                })
                .ToListAsync();

            return Ok(pendingOrders);
        }

        // ── Helpers ────────────────────────────────────────────────────────

        // Generates the next inward number: SRT-JCIN000829
        private static string GenerateInwardNo(string? latest)
        {
            int next = 1;
            if (latest != null)
            {
                var idx = latest.IndexOf("JCIN");
                if (idx >= 0 && int.TryParse(latest.Substring(idx + 4), out int parsed))
                    next = parsed + 1;
            }
            return $"SRT-JCIN{next:D6}";
        }

        private long GetWorkshopId() =>
            long.Parse(User.FindFirst("WorkshopId")?.Value ?? "0");

        private long GetUserId() =>
            long.Parse(User.FindFirst("UserId")?.Value ?? "0");

        [HttpGet("jobcard/search")]
        public async Task<IActionResult> SearchJobCards([FromQuery] string q = "")
        {
            if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
                return Ok(new List<object>());

            var workshopId = GetWorkshopId();
            var term = q.Trim().ToLower();

            var results = await _db.JobCards
                .Where(j =>
                    j.WorkshopId == workshopId &&
                    j.RowState == 1 &&
                    (
                        j.JobCardNo.ToLower().Contains(term) ||
                        j.RegistrationNo.ToLower().Contains(term) ||
                        j.CustomerName.ToLower().Contains(term) ||
                        j.Mobile.ToLower().Contains(term)
                    ))
                .OrderByDescending(j => j.ModifiedOn)
                .Take(15)
                .Select(j => new
                {
                    jobCardId = j.Id,
                    jobCardNo = j.JobCardNo,
                    regNo = j.RegistrationNo,
                    customerName = j.CustomerName,
                    mobileNo = j.Mobile,
                    status = j.Status
                })
                .ToListAsync();

            return Ok(results);
        }
    }
}