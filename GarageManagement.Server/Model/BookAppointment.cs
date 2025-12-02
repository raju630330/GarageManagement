using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class BookAppointment
    {
        [Key]
        [Required]
        public string search { get; set; }

        [Required]
        public DateTime? date { get; set; }

        [Required]
        public string time { get; set; }

        [Required]
        public string customerType { get; set; }

        [Required]
        public string regPrefix { get; set; }


        [ConditionalRequired("customerType", "Individual")]
        public string regNo { get; set; }

        [ConditionalRequired("customerType", "Individual")]
        public string vehicleType { get; set; }

        [ConditionalRequired("customerType", "Individual")]
        public string mobileNo { get; set; }

        [ConditionalRequired("customerType", "Individual")]
        public string emailID { get; set; }

        [ConditionalRequired("customerType", "Individual")]
        public string service { get; set; }

        [ConditionalRequired("customerType", "Individual")]
        public string serviceAdvisor { get; set; }

        [ConditionalRequired("customerType", "Individual")]
        public string settings { get; set; }

        [ConditionalRequired("customerType", "Individual")]
        public string bay { get; set; }

        //// Foreign Keys + Navigation
        //public int UserId { get; set; }
        //public User User { get; set; }   // Many-to-One (User → BookAppointments)

        //    public int WorkshopId { get; set; }
        //    public WorkshopProfile WorkshopProfile { get; set; } // Many-to-One (Workshop → BookAppointments)
        //}
    }
}


        
