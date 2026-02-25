using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class RepairOrder : BaseEntity
    {
        public string RegistrationNumber { get; set; } = string.Empty; 
        public string VinNumber { get; set; } = string.Empty;
        public string Mkls { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string VehicleSite { get; set; } = string.Empty;
        public string SiteInchargeName { get; set; } = string.Empty;
        public bool UnderWarranty { get; set; }
        public DateTime ExpectedDateTime { get; set; }
        public string AllottedTechnician { get; set; } = string.Empty;
        public long BookingAppointmentId { get; set; }
        public BookAppointment? BookAppointment { get; set; }
        public string Model { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public decimal RepairEstimationCost { get; set; }
        public bool DriverPermanetToThisVehicle { get; set; }
        public string TypeOfService { get; set; } = string.Empty;
        public bool RoadTestAlongWithDriver { get; set; }
        public InventoryForm? InventoryForm { get; set; }
        public ICollection<ToBeFilledBySupervisor> ToBeFilledBySupervisors { get; set; }  = new HashSet<ToBeFilledBySupervisor>();  
        public ICollection<AdditionalJobObserveDetail> AdditionalJobObserveDetails { get; set; } = new HashSet<AdditionalJobObserveDetail>();
        public ICollection<SparePartsIssueDetail> SparePartsIssueDetails { get; set; } = new HashSet<SparePartsIssueDetail>();  
        public ICollection<LabourDetail> LabourDetails { get; set; } = new HashSet<LabourDetail>();
        public TechnicianMC? TechnicianMC { get; set; }   
    }
}
