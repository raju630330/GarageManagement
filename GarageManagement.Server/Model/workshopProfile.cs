using GarageManagement.Server.Model;
using System.ComponentModel.DataAnnotations;

public class WorkshopProfile : BaseEntity
{
    public string WorkshopName { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerMobileNo { get; set; } = string.Empty;
    public string EmailID { get; set; } = string.Empty;

    public DateTime InBusinessSince { get; set; }
    public int AvgVehicleInflowPerMonth { get; set; }
    public int NoOfEmployees { get; set; }

    public string DealerCode { get; set; } = string.Empty;
    public bool IsGdprAccepted { get; set; } 
    public string ContactPerson { get; set; } = string.Empty;
    public string ContactNo { get; set; } = string.Empty;
    public string Landline { get; set; } = string.Empty;
    public DateTime? CalendarDate { get; set; }
    public WorkshopAddress? Address { get; set; }
    public WorkshopTiming? Timing { get; set; }
    public WorkshopBusinessConfig? WorkshopBusinessConfigs { get; set; }

    public ICollection<WorkshopService>? Services { get; set; }
    public ICollection<WorkshopWorkingDay>? WorkingDays { get; set; }
    public ICollection<WorkshopUser>? WorkshopUsers { get; set; }
    public ICollection<WorkshopMedia>? WorkshopMedias { get; set; }
    public ICollection<WorkshopPaymentMode>? WorkshopPaymentModes { get; set; }
    public ICollection<BookAppointment>? Appointments { get; set; } = new List<BookAppointment>();
    public long? ParentWorkshopId { get; set; }
    public WorkshopProfile? ParentWorkshop { get; set; }
    public ICollection<WorkshopProfile>? Branches { get; set; }
    public bool IsActive { get; set; } = true;
}
public class WorkshopAddress : BaseEntity
{
    public long WorkshopId { get; set; }
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
    public WorkshopProfile? Workshop { get; set; }   
}
public class WorkshopTiming : BaseEntity
{
    public long WorkshopId { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public WorkshopProfile? Workshop { get; set; }
}
public class Service : BaseEntity
{
    public string Name { get; set; } = string.Empty; 
}
public class WorkshopService : BaseEntity
{
    public long WorkshopId { get; set; }
    public long ServiceId { get; set; }
    public WorkshopProfile? Workshop { get; set; }
    public Service? Service { get; set; }
}
public class WorkshopWorkingDay : BaseEntity
{
    public long WorkshopId { get; set; }
    public DayOfWeek Day { get; set; }
    public WorkshopProfile? Workshop { get; set; }
}

public class WorkshopBusinessConfig : BaseEntity
{
    public long WorkshopId { get; set; }
    public string WebsiteLink { get; set; } = string.Empty;
    public string GoogleReviewLink { get; set; } = string.Empty;
    public string ExternalIntegrationUrl { get; set; } = string.Empty;
    public string GSTIN { get; set; } = string.Empty;
    public string MSME { get; set; } = string.Empty;
    public string SAC { get; set; } = string.Empty;
    public decimal? SACPercentage { get; set; }
    public string InvoiceCaption { get; set; } = string.Empty;
    public string InvoiceHeader { get; set; } = string.Empty;
    public string DefaultServiceType { get; set; } = string.Empty;       
    public WorkshopProfile? Workshop { get; set; }
}
public class PaymentMode : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}

public class WorkshopPaymentMode : BaseEntity
{
    public long WorkshopId { get; set; }
    public long PaymentModeId { get; set; }
    public WorkshopProfile? Workshop { get; set; }
    public PaymentMode? PaymentMode { get; set; }
}
public class WorkshopMedia : BaseEntity
{
    public long WorkshopId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public WorkshopProfile? Workshop { get; set; }
}
