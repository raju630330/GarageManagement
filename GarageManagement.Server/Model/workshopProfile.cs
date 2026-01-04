using GarageManagement.Server.Model;
using System.ComponentModel.DataAnnotations;

public class WorkshopProfile
{
    public long Id { get; set; }

    [Required]
    public string WorkshopName { get; set; }

    [Required]
    public string OwnerName { get; set; }

    [Required]
    public string OwnerMobileNo { get; set; }

    [Required]
    public string EmailID { get; set; }

    [Required]
    public string ContactPerson { get; set; }

    [Required]
    public string ContactNo { get; set; }

    [Required]
    public string Landline { get; set; }

    [Required]
    public DateTime? CalendarDate { get; set; }

    // Navigation
    // Navigation: One Workshop → Many Appointments
    public ICollection<BookAppointment> Appointments { get; set; } = new List<BookAppointment>();

    public ICollection<WorkshopUser> WorkshopUsers { get; set; }
}
