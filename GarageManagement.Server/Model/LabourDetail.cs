using System.ComponentModel.DataAnnotations.Schema;

namespace GarageManagement.Server.Model
{
    public class LabourDetail : BaseEntity
    {
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal LabourCharges { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OutsideLabour { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public long RepairOrderId { get; set; }
        public RepairOrder? RepairOrder { get; set; }
    }
}
