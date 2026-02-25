using GarageManagement.Server.Model;

namespace GarageManagement.Server.dtos
{
    public class JobCardDto:BaseResultDto
    {
        public long RepairOrderId { get; set; }
        public VehicleDataDto VehicleData { get; set; } = new VehicleDataDto();
        public List<ConcernDto> Concerns { get; set; } = new List<ConcernDto>();
        public CustomerInfoDto CustomerInfo { get; set; } = new CustomerInfoDto();
        public AdvancePaymentDto AdvancePayment { get; set; } = new AdvancePaymentDto();
    }
    public class EstimationSaveDto:BaseResultDto
    {
        public long JobCardId { get; set; } 

        public EstimationDto Estimation { get; set; } = new EstimationDto(); 
        public PopupDto Popup { get; set; } = new PopupDto(); 

    }
    public class GetEstimationDto : BaseResultDto
    {
        public long JobCardId { get; set; }
        public string Status { get; set; } = string.Empty;
        public VehicleDataDto VehicleData { get; set; }  = new VehicleDataDto();
        public CustomerInfoDto CustomerInfo { get; set; } = new CustomerInfoDto();
        public EstimationDto Estimation { get; set; } = new EstimationDto(); 
        public PopupDto Popup { get; set; } = new PopupDto(); 

    }
    public class EstimationDto
    {
        public List<EstimationItemDto> Items { get; set; } = new List<EstimationItemDto>();
        public decimal DiscountInput { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal BalanceAmount { get; set; }
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
        public string Approval { get; set; } = string.Empty;
    }

    public class PopupDto
    {
        public List<TyreBatteryDto> TyreBattery { get; set; } = new List<TyreBatteryDto>();
        public List<CancelledInvoiceDto> CancelledInvoices { get; set; } = new List<CancelledInvoiceDto>();
        public List<CollectionDto> Collections { get; set; } = new List<CollectionDto>();
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
    public class JobCardListDto
    {
        public string RefNo { get; set; } = string.Empty;
        public string JobCardNo { get; set; } = string.Empty;
        public string RegNo { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string Vehicle { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public DateTime ArrivalDate { get; set; } 
        public string ArrivalTime { get; set; } = string.Empty;
        public string InsuranceCorporate { get; set; } = string.Empty;
        public string ClaimNo { get; set; } = string.Empty;
        public DateTime? EstDeliveryDate { get; set; }
        public DateTime AccidentDate { get; set; }
    }
    public class EstimationItemsSaveDto 
    {
        public long Id { get; set; }
        public long JobCardId { get; set; }
        public long PartId { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public string HSN { get; set; } = string.Empty;
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public string Approval { get; set; } = string.Empty;

    }
    public class PendingIssueItemDto
    {
        public long EstimationItemId { get; set; }

        public string PartNo { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;

        public decimal QtyOnHand { get; set; }      // Total stock available
        public decimal RequestedQty { get; set; }   // From estimation
        public decimal IssuedQty { get; set; }      // Already issued
        public decimal PendingQty { get; set; }     // Requested - Issued
        public decimal SellingPrice { get; set; }

        public bool InStock { get; set; }           // true if QtyOnHand > 0

        // Optional: These can be filled later when issued
        public decimal IssueQty { get; set; }       // Qty being issued now
        public string IssuedTo { get; set; } = string.Empty;
    }

    public class IssuePartsRequestDto
    {
        public long JobCardId { get; set; }
        public List<IssuePartItemDto> Items { get; set; } = new();
    }

    public class IssuePartItemDto
    {
        public long EstimationItemId { get; set; }
        public int IssueQty { get; set; }
    }

    public class IssuedItemDto
    {
        public long EstimationItemId { get; set; }
        public string PartNo { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal RequestedQty { get; set; }
        public decimal IssuedQty { get; set; }
        public decimal ReturnQty { get; set; } = 0; 
        public decimal UnitPrice { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string? IssuedTo { get; set; }
        public string IssuedId { get; set; } = string.Empty;
    }

    public class ReturnPartsRequestDto
    {
        public long JobCardId { get; set; }
        public List<ReturnPartItemDto> Items { get; set; } = new();
    }
    public class ReturnPartItemDto
    {
        public long EstimationItemId { get; set; }
        public int ReturnQty { get; set; }
    }

    public class ReturnItemDto
    {
        public long EstimationItemId { get; set; }

        public long? PartId { get; set; }
        public string PartName { get; set; } = string.Empty;
        public string PartNo { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;

        public decimal RequestedQty { get; set; }
        public decimal IssuedQty { get; set; }
        public decimal ReturnQty { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime? IssuedDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string IssuedTo { get; set; } = string.Empty;
    }


}
