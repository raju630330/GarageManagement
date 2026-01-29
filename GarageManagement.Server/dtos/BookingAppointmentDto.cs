using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.dtos
{
    public class BookingAppointmentDto
    {
        public long Id { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(5)] // HH:mm format
        public string AppointmentTime { get; set; } = string.Empty;

        public string Bay { get; set; } = string.Empty;

        [Required]
        public long CustomerId { get; set; }

        [Required]
        public string CustomerType { get; set; } = string.Empty;    // e.g., Individual or Corporate
        public string Service { get; set; } = string.Empty;
        public string ServiceAdvisor { get; set; } = string.Empty;

        [Required]
        public long UserId { get; set; }

        public long? VehicleId { get; set; }

        [Required]
        public long WorkshopId { get; set; }
        public string RegPrefix { get; set; } = string.Empty;
        public string RegNo { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
