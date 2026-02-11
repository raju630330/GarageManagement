namespace GarageManagement.Server.Model
{
    public class StockMovement
    {
        public long Id { get; set; }

        public long PartId { get; set; }
        public Part Part { get; set; } = null!;
        public long? EstimationItemId { get; set; }   // 🔑 key relation
        public JobCardEstimationItem EstimationItem { get; set; } = null!;
        public decimal Quantity { get; set; }  // +ve Inward, -ve Outward

        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }

        public string TransactionType { get; set; } = string.Empty;
        // INWARD, ISSUE, RETURN, ADJUSTMENT

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public string Barcode { get; set; } = string.Empty;
        public long? UserId { get; set; }
        public User User { get; set; } = null!;

    }

}
