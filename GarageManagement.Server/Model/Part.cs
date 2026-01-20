namespace GarageManagement.Server.Model
{
    public class Part
    {
        public long Id { get; set; }

        public string PartNo { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public string TaxType { get; set; } = "GST";
        public decimal TaxPercent { get; set; }

        public string RackNo { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }

}
