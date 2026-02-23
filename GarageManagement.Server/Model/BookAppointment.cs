using GarageManagement.Server.Model;
using System.ComponentModel.DataAnnotations;

public class BookAppointment : BaseEntity
{
    public DateTime AppointmentDate { get; set; }
    public string AppointmentTime { get; set; } = string.Empty;
    public string CustomerType { get; set; } = string.Empty;

    public string Service { get; set; } = string.Empty;
    public string ServiceAdvisor { get; set; } = string.Empty;
    public string Bay { get; set; } = string.Empty;
    public long CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public long? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public long? UserId { get; set; } 
    public User? User { get; set; }

    public long? WorkshopId { get; set; }
    public WorkshopProfile? Workshop { get; set; }

}
