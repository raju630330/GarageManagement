using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class Vehicle
    {
        public long Id { get; set; }

        public long CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required]
        public string RegPrefix { get; set; }

        [Required]
        public string RegNo { get; set; }

        [Required]
        public string VehicleType { get; set; }

        // Navigation
        public ICollection<BookAppointment> Appointments { get; set; }
    }
}
