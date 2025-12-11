namespace GarageManagement.Server.Model
{
    public class JobCard
    {
        public int Id { get; set; }

        // --- Vehicle Data ---
        public string? RegistrationNo { get; set; }
        public long? OdometerIn { get; set; }
        public long? AvgKmsPerDay { get; set; }
        public string? Vin { get; set; }
        public string? EngineNo { get; set; }
        public string? VehicleColor { get; set; }
        public string? FuelType { get; set; }
        public string? ServiceType { get; set; }
        public string? ServiceAdvisor { get; set; }
        public string? Technician { get; set; }
        public string? Vendor { get; set; }

        // --- Customer Info ---
        public string? Corporate { get; set; }
        public string? CustomerName { get; set; }
        public string? Mobile { get; set; }
        public string? AlternateMobile { get; set; }
        public string? Email { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? InsuranceCompany { get; set; }

        // --- Navigation ---
        public JobCardAdvancePayment? AdvancePayment { get; set; }
        public ICollection<JobCardConcern>? Concerns { get; set; }
    }
}
