
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.dtos
{
    // ── Request: POST /api/inward ──────────────────────────────────────────
    public class CreateInwardDto
    {
        // Optional — only present when opened from a PurchaseOrder
        public long? OrderId { get; set; }
        public long? JobCardId { get; set; }
        public string RegNo { get; set; } = "STOCK";
        public string JobCardNo { get; set; } = string.Empty;

        // Header fields (mandatory *)
        public string DeliveryReceipt { get; set; } = string.Empty;

        [Required]
        public string BillNo { get; set; } = string.Empty;

        [Required]
        public DateTime BillDate { get; set; }

        [Required]
        public string TaxType { get; set; } = string.Empty;   // GST | VAT

        [Required]
        public long SupplierId { get; set; }

        // Summary charges
        public decimal FreightAmount { get; set; } = 0;
        public decimal TcsAmount { get; set; } = 0;
        public decimal PaidAmount { get; set; } = 0;

        [Required, MinLength(1)]
        public List<InwardItemDto> Items { get; set; } = new();
    }

    public class InwardItemDto
    {
        [Required]
        public long PartId { get; set; }

        // nullable — only set when linked to a PO item
        public long? PurchaseOrderItemId { get; set; }

        [Required, Range(0.0001, double.MaxValue)]
        public decimal InwardQty { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Discount { get; set; } = 0;

        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPurchasePrice { get; set; }

        [Required, Range(0.0001, double.MaxValue)]
        public decimal SellingPrice { get; set; }

        public string RackNo { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }

    // ── Response: POST /api/inward ─────────────────────────────────────────
    public class CreateInwardResponse
    {
        public bool IsSuccess { get; set; }
        public string InwardNo { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    // ── Response: GET /api/inward (list row) ──────────────────────────────
    // Matches Inward tab columns from document pages 54-56
    public class InwardListDto
    {
        public long Id { get; set; }
        public string InwardNo { get; set; } = string.Empty;  // SRT-JCIN000829
        public string InwardDate { get; set; } = string.Empty;  // dd-MM-yyyy
        public string OrderNo { get; set; } = string.Empty;
        public string JobCardNo { get; set; } = string.Empty;
        public string RegNo { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public string BillNo { get; set; } = string.Empty;
        public decimal InwardValue { get; set; }
    }

    // ── Filter for GET /api/inward ─────────────────────────────────────────
    public class InwardFilterDto
    {
        public string? Search { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}