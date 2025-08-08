using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class WorkshopProfile
    {
        //[Key]
        //[Required]
        //public int Id { get; set; }
        [Key]
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
    }
    
}


