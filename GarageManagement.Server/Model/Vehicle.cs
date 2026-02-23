using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class Vehicle : BaseEntity
    {
        public long CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public string RegPrefix { get; set; } = string.Empty;
        public string RegNo { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public ICollection<BookAppointment>? Appointments { get; set; }
    }
}
