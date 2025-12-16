namespace GarageManagement.Server.dtos
{
    public class JobCardDto
    {
        public  long Id { get; set; }
        public VehicleDataDto VehicleData { get; set; }
        public List<ConcernDto> Concerns { get; set; }
        public CustomerInfoDto CustomerInfo { get; set; }
        public AdvancePaymentDto AdvancePayment { get; set; }
    }
    public class JobCardSaveDto
    {
        public long JobCardId { get; set; } // Id of the job card to update
        public EstimationDto Estimation { get; set; } = new EstimationDto(); // Estimation data
        public PopupDto Popup { get; set; } = new PopupDto(); // Popup/tab data

    }

    public class EstimationDto
    {
        public List<EstimationItemDto> Items { get; set; } = new();
        public decimal DiscountInput { get; set; }
        public decimal PaidAmount { get; set; }
    }

    public class EstimationItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string PartNo { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public string HSN { get; set; } = string.Empty;
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
    }

    public class PopupDto
    {
        public List<TyreBatteryDto> TyreBattery { get; set; } = new();
        public List<CancelledInvoiceDto> CancelledInvoices { get; set; } = new();
        public List<CollectionDto> Collections { get; set; } = new();
        public string ServiceSuggestions { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }

    public class TyreBatteryDto
    {
        public string Type { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Condition { get; set; } = string.Empty;
    }

    public class CancelledInvoiceDto
    {
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }

    public class CollectionDto
    {
        public string Type { get; set; } = string.Empty;
        public string Bank { get; set; } = string.Empty;
        public string ChequeNo { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }
}
