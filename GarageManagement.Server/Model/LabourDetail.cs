using System.ComponentModel.DataAnnotations.Schema;

namespace GarageManagement.Server.Model
{
    public class LabourDetail
    {
        public int Id { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LabourCharges { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OutsideLabour { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
