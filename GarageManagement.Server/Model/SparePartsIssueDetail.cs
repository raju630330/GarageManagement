using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageManagement.Server.Model
{
    public class SparePartsIssueDetail : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public decimal UnitCost { get; set; }
        public int Quantity { get; set; }
        public long? RepairOrderId { get; set; }
        public RepairOrder? RepairOrder { get; set; }
    }
}
