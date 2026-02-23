namespace GarageManagement.Server.Model
{
    public class JobCardEstimationItem : BaseEntity
    {
        public long JobCardId { get; set; }
        public JobCard JobCard { get; set; } = null!;
        public long? PartId { get; set; }
        public Part? Part { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal RequestedQuantity { get; set; }    
        public decimal IssuedQty { get; set; }     
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public string HSN { get; set; } = string.Empty;
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }

        public string ApprovalStatus { get; set; } = string.Empty;
        public DateTime? IssuedDate { get; set; }
        public string? IssuedBy { get; set; }  
        public string? IssuedId { get; set; }
        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    }
}
