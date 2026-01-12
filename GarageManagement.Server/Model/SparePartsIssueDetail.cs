using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageManagement.Server.Model
{
    public class SparePartsIssueDetail
    {
        public long Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PartNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Make { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitCost { get; set; }

        [Required]
        public int Quantity { get; set; }
        public long? RepairOrderId { get; set; }
        public RepairOrder RepairOrder { get; set; }
    }
}
