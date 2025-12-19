namespace GarageManagement.Server.dtos
{
    public class VehicleDataDto
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
    }
}
