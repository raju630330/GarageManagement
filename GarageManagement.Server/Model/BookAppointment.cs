using GarageManagement.Server.Model;
using System.ComponentModel.DataAnnotations;

public class BookAppointment
{
    public long Id { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public string AppointmentTime { get; set; }

    [Required]
    public string CustomerType { get; set; } // Individual / Corporate

    public string Service { get; set; }
    public string ServiceAdvisor { get; set; }
    public string Bay { get; set; }

    // Foreign Keys
    public long CustomerId { get; set; }
    public Customer Customer { get; set; }

    public long? VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }

    public long? UserId { get; set; } // Staff handling
    public User User { get; set; }

    public long? WorkshopId { get; set; }
    public WorkshopProfile Workshop { get; set; }

}
