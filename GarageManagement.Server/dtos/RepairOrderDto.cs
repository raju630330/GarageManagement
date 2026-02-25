using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.dtos
{
    public class RepairOrderDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Registration Number is required")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "VIN Number is required")]
        public string VinNumber { get; set; } = string.Empty;

        public string Mkls { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        public string Make { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"\+91\d{10}", ErrorMessage = "Phone number must start with +91 and be followed by 10 digits.")]
        public string Phone { get; set; } = string.Empty;

        public string VehicleSite { get; set; } = string.Empty;

        public string SiteInchargeName { get; set; } = string.Empty;

        public bool UnderWarranty { get; set; }

        public DateTime ExpectedDateTime { get; set; }

        public string AllottedTechnician { get; set; } = string.Empty;
        public long BookingAppointmentId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public decimal RepairEstimationCost { get; set; }
        public bool DriverPermanetToThisVehicle { get; set; }
        public string TypeOfService { get; set; } = string.Empty;
        public bool RoadTestAlongWithDriver { get; set; }
    }
}
