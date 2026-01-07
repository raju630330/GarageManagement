using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.dtos
{
    public class BookingAppointmentDto
    {
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(5)] // HH:mm format
        public string AppointmentTime { get; set; }

        public string Bay { get; set; }

        [Required]
        public long CustomerId { get; set; }

        [Required]
        public string CustomerType { get; set; } // e.g., Individual or Corporate
        public string Service { get; set; }
        public string ServiceAdvisor { get; set; }

        [Required]
        public long UserId { get; set; }

        public long? VehicleId { get; set; }

        [Required]
        public long WorkshopId { get; set; }
    }
}
