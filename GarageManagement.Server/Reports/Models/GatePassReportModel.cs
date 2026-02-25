namespace GarageManagement.Server.Reports.Models
{
    public class GatePassReportModel
    {
        public string CustomerName { get; set; } = string.Empty;    
        public DateTime VehicleInDate { get; set; }
        public DateTime VehicleOutDate { get; set; }
        public string JobCardNo { get; set; } = string.Empty;
        public string RegistrationNo { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;
        public string Advisor { get; set; } = string.Empty;
        public long OdometerIn { get; set; }
        public long OdemeterOut { get; set; }
    }
}
