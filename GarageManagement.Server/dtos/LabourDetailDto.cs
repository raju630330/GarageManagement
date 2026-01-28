using System.ComponentModel.DataAnnotations.Schema;

namespace GarageManagement.Server.dtos
{
    public class LabourDetailDto
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public decimal LabourCharges { get; set; }
        public decimal OutsideLabour { get; set; }
        public decimal Amount { get; set; }
        public long? RepairOrderId { get; set; }
    }
}
