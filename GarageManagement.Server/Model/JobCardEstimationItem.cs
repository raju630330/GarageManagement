namespace GarageManagement.Server.Model
{
    public class JobCardEstimationItem
    {
        public long Id { get; set; }

        public long JobCardId { get; set; }
        public JobCard JobCard { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string PartNo { get; set; } = string.Empty;

        public decimal Rate { get; set; }
        public decimal Discount { get; set; }

        public string HSN { get; set; } = string.Empty;
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }

        public string ApprovalStatus { get; set; } = string.Empty;
    }
}
