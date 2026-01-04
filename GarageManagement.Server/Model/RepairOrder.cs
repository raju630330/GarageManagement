using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class RepairOrder
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Registration Number is required")]
        public string? RegistrationNumber { get; set; } 

        [Required(ErrorMessage = "VIN Number is required")]
        public string? VinNumber { get; set; }

        public string? Mkls { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        public string? Make { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"\+91\d{10}", ErrorMessage = "Phone number must start with +91 and be followed by 10 digits.")]
        public string? Phone { get; set; }

        public string? VehicleSite { get; set; }

        public string? SiteInchargeName { get; set; }

        public bool UnderWarranty { get; set; }

        public DateTime? ExpectedDateTime { get; set; }

        public string? AllottedTechnician { get; set; }
    }
}
