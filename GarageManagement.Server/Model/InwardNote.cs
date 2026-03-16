
using System;
using System.Collections.Generic;

namespace GarageManagement.Server.Model
{
    // ── InwardNote (header) ────────────────────────────────────────────────
    // One record per inward transaction.
    // InwardNo pattern: SRT-JCIN000001
    public class InwardNote : BaseEntity
    {

        // e.g. SRT-JCIN000829
        public string InwardNo { get; set; } = string.Empty;

        // nullable — not all inwards are linked to a PO
        public long? PurchaseOrderId { get; set; }

        // nullable — not all inwards are linked to a Job Card
        public long? JobCardId { get; set; }

        // Vehicle reg or "STOCK"
        public string RegNo { get; set; } = "STOCK";

        public string JobCardNo { get; set; } = string.Empty;

        // Header form fields (pages 46-47 of doc)
        public string DeliveryReceipt { get; set; } = string.Empty;

        public string BillNo { get; set; } = string.Empty;   // mandatory *
        public DateTime BillDate { get; set; }                  // mandatory *
        public string TaxType { get; set; } = string.Empty;   // mandatory * (GST / VAT)

        public long SupplierId { get; set; }                   // mandatory *

        // Summary section (page 48)
        public decimal FreightAmount { get; set; } = 0;
        public decimal TcsAmount { get; set; } = 0;
        public decimal PaidAmount { get; set; } = 0;

        // Navigation
        public Supplier? Supplier { get; set; }
        public ICollection<InwardNoteItem> Items { get; set; } = new List<InwardNoteItem>();
    }

    // ── InwardNoteItem (one row per part) ─────────────────────────────────
    // Table columns from document pages 47-48
    public class InwardNoteItem : BaseEntity
    {

        public long InwardNoteId { get; set; }

        public long PartId { get; set; }

        // nullable — only set when inward came from a PurchaseOrder
        public long? PurchaseOrderItemId { get; set; }

        // Editable columns in the table
        public decimal InwardQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Margin { get; set; }   // SellingPrice - UnitPrice

        public string RackNo { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;

        // Navigation
        public InwardNote? InwardNote { get; set; }
    }
}