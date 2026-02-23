using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class Customer : BaseEntity
    {
        public string CustomerName { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public ICollection<Vehicle>? Vehicles { get; set; }
        public ICollection<BookAppointment>? Appointments { get; set; }
    }
}
