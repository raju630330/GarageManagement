using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class WorkshopProfile
    {
        [Key]  
        public int WorkshopId { get; set; }

        [Required(ErrorMessage="Workshop Name is required")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "No special characters allowed. Only alphabets and spaces.")]
        public string WorkshopName { get; set; }

        [Required(ErrorMessage = "OwnerName Name is required")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "No special characters allowed. Only alphabets and spaces.")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "OwnerMobileNo is required")]
        [RegularExpression(@"^\+91\d{10}$", ErrorMessage = "Phone number must start with +91 and be followed by 10 digits.")]
        public string OwnerMobileNo { get; set; }

        [Required]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "ContactPerson Name is required")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "No special characters allowed. Only alphabets and spaces.")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "ContactNo is required")]
        [RegularExpression(@"^\+91\d{10}$", ErrorMessage = "Phone number must start with +91 and be followed by 10 digits.")]
        public string ContactNo { get; set; }

        [Required]
        public string Landline { get; set; }

        [Required]
        public DateTime? CalendarDate { get; set; }

        //// Navigation: One Workshop → Many Appointments
        //public ICollection<BookAppointment> BookAppointments { get; set; }
    }
    
}


