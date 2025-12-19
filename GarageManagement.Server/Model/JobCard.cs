namespace GarageManagement.Server.Model
{
    public class JobCard
    {
        public long Id { get; set; }

        // Vehicle
        public string RegistrationNo { get; set; } = string.Empty;
        public string? JobCardNo { get; set; } = string.Empty;
        public long? OdometerIn { get; set; }
        public long? AvgKmsPerDay { get; set; }
        public string Vin { get; set; } = string.Empty;
        public string EngineNo { get; set; } = string.Empty;
        public string VehicleColor { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string ServiceAdvisor { get; set; } = string.Empty;
        public string Technician { get; set; } = string.Empty;
        public string Vendor { get; set; } = string.Empty;

        // Customer
        public string Corporate { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string AlternateMobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? DeliveryDate { get; set; }
        public string InsuranceCompany { get; set; } = string.Empty;

        // Popup (single column)
        public decimal Discount { get; set; }
        public decimal Paid { get; set; }
        public string? ServiceSuggestions { get; set; } = string.Empty;
        public string? Remarks { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;

        // Navigation
        public JobCardAdvancePayment? AdvancePayment { get; set; }
        public ICollection<JobCardConcern> Concerns { get; set; } = new List<JobCardConcern>();
        public ICollection<JobCardEstimationItem>? JobCardEstimationItems { get; set; }
        public ICollection<JobCardTyreBattery>? TyreBatteries { get; set; }
        public ICollection<JobCardCancelledInvoice>? CancelledInvoices { get; set; }
        public ICollection<JobCardCollection>? Collections { get; set; }

    }
}
