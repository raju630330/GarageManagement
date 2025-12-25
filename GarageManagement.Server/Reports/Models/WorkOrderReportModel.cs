namespace GarageManagement.Server.Reports.Models
{
    public class WorkOrderReportModel
    {
        public string? JobCardNo { get; set; }
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
        public bool IsApproved { get; set; }
    }
}

