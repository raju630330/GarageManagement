using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class Customer
    {
        public long Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string MobileNo { get; set; }

        public string Email { get; set; }

        [Required]
        public string CustomerType { get; set; } // Individual / Corporate

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<BookAppointment> Appointments { get; set; }
    }
}
