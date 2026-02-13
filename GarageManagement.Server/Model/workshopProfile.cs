using GarageManagement.Server.Model;
using System.ComponentModel.DataAnnotations;

public class WorkshopProfile
{
    public long Id { get; set; }
    public string WorkshopName { get; set; }
    public string OwnerName { get; set; }
    public string OwnerMobileNo { get; set; }
    public string EmailID { get; set; }

    public DateTime InBusinessSince { get; set; }
    public int AvgVehicleInflowPerMonth { get; set; }
    public int NoOfEmployees { get; set; }

    public string DealerCode { get; set; }
    public bool IsGdprAccepted { get; set; } 
    public string ContactPerson { get; set; }
    public string ContactNo { get; set; }
    public string Landline { get; set; }
    public DateTime? CalendarDate { get; set; }
    public WorkshopAddress Address { get; set; }
    public WorkshopTiming Timing { get; set; }
    public WorkshopBusinessConfig WorkshopBusinessConfigs { get; set; }

    public ICollection<WorkshopService> Services { get; set; }
    public ICollection<WorkshopWorkingDay> WorkingDays { get; set; }
    public ICollection<WorkshopUser> WorkshopUsers { get; set; }
    public ICollection<WorkshopMedia> WorkshopMedias { get; set; }
    public ICollection<WorkshopPaymentMode> WorkshopPaymentModes { get; set; }
    public ICollection<BookAppointment> Appointments { get; set; } = new List<BookAppointment>();
}
public class WorkshopAddress
{
    public long Id { get; set; }
    public long WorkshopId { get; set; }

    public string FlatNo { get; set; }
    public string Street { get; set; }
    public string Location { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string StateCode { get; set; }
    public string Country { get; set; }
    public string Pincode { get; set; }
    public string Landmark { get; set; }
    public string BranchAddress { get; set; }

    public WorkshopProfile Workshop { get; set; }
}
public class WorkshopTiming
{
    public long Id { get; set; }
    public long WorkshopId { get; set; }

    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public WorkshopProfile Workshop { get; set; }
}
public class Service
{
    public long Id { get; set; }
    public string Name { get; set; } // WiFi, RestRoom, FreeCoffee
}
public class WorkshopService
{
    public long WorkshopId { get; set; }
    public long ServiceId { get; set; }

    public WorkshopProfile Workshop { get; set; }
    public Service Service { get; set; }
}
public class WorkshopWorkingDay
{
    public long Id { get; set; }
    public long WorkshopId { get; set; }

    public DayOfWeek Day { get; set; }

    public WorkshopProfile Workshop { get; set; }
}

public class WorkshopBusinessConfig
{
    public long Id { get; set; }
    public long WorkshopId { get; set; }

    public string WebsiteLink { get; set; }
    public string GoogleReviewLink { get; set; }
    public string ExternalIntegrationUrl { get; set; }

    public string GSTIN { get; set; }
    public string MSME { get; set; }
    public string SAC { get; set; }
    public decimal? SACPercentage { get; set; }

    public string InvoiceCaption { get; set; }
    public string InvoiceHeader { get; set; }
    public string DefaultServiceType { get; set; }

    public WorkshopProfile Workshop { get; set; }
}
public class PaymentMode
{
    public long Id { get; set; }
    public string Name { get; set; }
}

public class WorkshopPaymentMode
{
    public long WorkshopId { get; set; }
    public long PaymentModeId { get; set; }

    public WorkshopProfile Workshop { get; set; }
    public PaymentMode PaymentMode { get; set; }
}
public class WorkshopMedia
{
    public long Id { get; set; }
    public long WorkshopId { get; set; }

    public string FilePath { get; set; }
    public string MediaType { get; set; }
    // Logo | AdditionalLogo | WorkshopImage

    public WorkshopProfile Workshop { get; set; }
}
