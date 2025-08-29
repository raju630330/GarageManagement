using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class WorkshopProfile
    {
        [Key]  
        public int WorkshopId { get; set; }
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

        // Navigation: One Workshop → Many Appointments
        public ICollection<BookAppointment> BookAppointments { get; set; }
    }
    
}


