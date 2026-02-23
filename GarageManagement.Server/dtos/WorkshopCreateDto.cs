namespace GarageManagement.Server.dtos
{
    public class WorkshopCreateDto
    {
        public long Id { get; set; }
        public string WorkshopName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string OwnerMobileNo { get; set; } = string.Empty;
        public string? EmailID { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactNo { get; set; }
        public string? Landline { get; set; }

        // ✅ FIXED: string instead of DateTime
        public string InBusinessSince { get; set; } = string.Empty;
        public int? AvgVehicleInflowPerMonth { get; set; }
        public int? NoOfEmployees { get; set; }
        public string? DealerCode { get; set; }
        public bool IsGdprAccepted { get; set; } = false;

        public WorkshopAddressDto Address { get; set; } = new();
        public WorkshopTimingDto Timing { get; set; } = new();
        public WorkshopBusinessConfigDto BusinessConfig { get; set; } = new();

        // ✅ FIXED: List<string> to match Angular empty arrays
        public List<string> ServiceIds { get; set; } = new();
        public List<string> WorkingDays { get; set; } = new();
        public List<string> PaymentModeIds { get; set; } = new();
        // public List<string> Media { get; set; } = new(); // Simple strings for now

        public List<IFormFile> MediaFiles { get; set; } = new();
    }


    public class WorkshopAddressDto
    {
        public string FlatNo { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string StateCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string Landmark { get; set; } = string.Empty;
        public string BranchAddress { get; set; } = string.Empty;
    }

    public class WorkshopTimingDto
    {
        public string StartTime { get; set; } = string.Empty;    // ✅ string
        public string EndTime { get; set; } = string.Empty;      // ✅ string
    }


    public class WorkshopBusinessConfigDto
    {
        public string WebsiteLink { get; set; } = string.Empty;
        public string GoogleReviewLink { get; set; } = string.Empty;
        public string ExternalIntegrationUrl { get; set; } = string.Empty;
        public string Gstin { get; set; } = string.Empty;      // ✅ camelCase
        public string Msme { get; set; } = string.Empty;       // ✅ camelCase  
        public string Sac { get; set; } = string.Empty;        // ✅ camelCase
        public decimal? SacPercentage { get; set; }             // ✅ string
        public string InvoiceCaption { get; set; } = string.Empty;
        public string InvoiceHeader { get; set; } = string.Empty;
        public string DefaultServiceType { get; set; } = string.Empty;
    }


    public class WorkshopMediaDto
    {
        public string FilePath { get; set; } = string.Empty;
        public string MediaType { get; set; } = string.Empty;
    }

    public class AssignUserToWorkshopDto
    {
        public long Id { get; set; }
        public long WorkshopId { get; set; }
        public long UserId { get; set; }
    }
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public List<T> Data { get; set; } = new();
    }
    public class WorkshopListDto
    {
        public long Id { get; set; }
        public string WorkshopName { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string OwnerMobileNo { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Gstin { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
