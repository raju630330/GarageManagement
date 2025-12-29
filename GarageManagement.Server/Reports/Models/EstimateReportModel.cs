namespace GarageManagement.Server.Reports.Models
{
    public class EstimateReportModel
    {
        public DateTime Date { get; set; }
        public string TicketNo { get; set; }
        public string VehicleNo { get; set; }
        public string VINNo { get; set; }
        public string EngineNo { get; set; }
        public string AdvisorName { get; set; }
        public string ServiceType { get; set; }
        public string Technician { get; set; }
        public string Vendor { get; set; }
    }
}
