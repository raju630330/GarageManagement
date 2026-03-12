namespace GarageManagement.Server.Model
{
        public class Supplier : BaseEntity
    {

            public string Name { get; set; } = string.Empty;

            public string ContactPerson { get; set; } = string.Empty;

            public string Phone { get; set; } = string.Empty;

            public string Email { get; set; } = string.Empty;

            public string GSTIN { get; set; } = string.Empty;

            public string Address { get; set; } = string.Empty;

            public bool IsActive { get; set; } = true;

            // Navigation
            public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
        }
    public class PurchaseOrder : BaseEntity
    {

        // e.g. SRT-STOR000010
        public string OrderNo { get; set; } = string.Empty;

        public long SupplierId { get; set; }

        // "Cash" | "Credit"
        public string PaymentType { get; set; } = string.Empty;

        // "JobCard" | "General" | ""
        public string StockType { get; set; } = string.Empty;

        public string Remarks { get; set; } = string.Empty;

        // null for pure stock orders
        public long? JobCardId { get; set; }

        // Vehicle reg or "STOCK"
        public string RegNo { get; set; } = "STOCK";

        public string JobCardNo { get; set; } = string.Empty;

        // estimation | issue | inward | psf | upload | ""
        public string Source { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        // PENDING | SHIPMENT | CLOSED | CANCELLED
        public string Status { get; set; } = "PENDING";

        // Navigation
        public Supplier? Supplier { get; set; }
        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }
    public class PurchaseOrderItem : BaseEntity
    {

        public long PurchaseOrderId { get; set; }

        public long PartId { get; set; }

        public decimal Qty { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }

        // Copied from Part.TaxPercent at order time
        public decimal TaxPercent { get; set; }

        // Computed: (Qty * UnitPrice - Discount) * TaxPercent / 100
        public decimal TaxAmount { get; set; }

        // Computed: (Qty * UnitPrice - Discount) + TaxAmount
        public decimal TotalPurchasePrice { get; set; }

        // "Part" | "Labour"
        public string ServiceType { get; set; } = "Part";

        public string Remarks { get; set; } = string.Empty;

        public string SellerInfo { get; set; } = string.Empty;

        // Updated when items are received via Inward
        public decimal InwardedQty { get; set; } = 0;

        // FK to StockMovement — set after inward
        public long? StockMovementId { get; set; }

        // Navigation
        public PurchaseOrder? PurchaseOrder { get; set; }
    }
}
