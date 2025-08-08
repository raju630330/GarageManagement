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
            public string state { get; set; }

            [Required]
            public string regNo { get; set; }

            [Required]
            public string vehicle { get; set; }

            [Required]
            public string mobileNo { get; set; }

            [Required]
            public string emailID { get; set; }

            [Required]
            public string service { get; set; }

            [Required]
            public string serviceAdvisor { get; set; }

            [Required]
            public string settings { get; set; }

            [Required]
            public string bay { get; set; }
        
    }
}
