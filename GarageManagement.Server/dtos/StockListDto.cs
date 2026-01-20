namespace GarageManagement.Server.dtos
{
    public class StockListDto
    {
        public string PartNo { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal QtyOnHand { get; set; }
        public decimal AvgPurchasePrice { get; set; }
        public decimal AvgSellingPrice { get; set; }
        public string TaxType { get; set; } = string.Empty;
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public string RackNo { get; set; } = string.Empty;
        public int Ageing { get; set; }
        public string Barcode { get; set; } = string.Empty;
    }
}
