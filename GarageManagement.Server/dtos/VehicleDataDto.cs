namespace GarageManagement.Server.dtos
{
    public class VehicleDataDto
    {
        public string JobCardNo { get; set; } = string.Empty;
        public string RegistrationNo { get; set; } = string.Empty;
        public long OdometerIn { get; set; }
        public long AvgKmsPerDay { get; set; }
        public string Vin { get; set; } = string.Empty;
        public string EngineNo { get; set; } = string.Empty;
        public string VehicleColor { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string ServiceAdvisor { get; set; } = string.Empty;
        public string Technician { get; set; } = string.Empty;
        public string Vendor { get; set; } = string.Empty;
    }
}
