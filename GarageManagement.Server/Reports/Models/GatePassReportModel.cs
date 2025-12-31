namespace GarageManagement.Server.Reports.Models
{
    public class GatePassReportModel
    {
        public string CustomerName { get; set; }
        public DateTime VehicleInDate { get; set; }
        public DateTime VehicleOutDate { get; set; }
        public string JobCardNo { get; set; }
        public string RegistrationNo { get; set; }
        public string InvoiceNo { get; set; }
        public string Advisor { get; set; }
        public long OdometerIn { get; set; }
        public long OdemeterOut { get; set; }
    }
}
