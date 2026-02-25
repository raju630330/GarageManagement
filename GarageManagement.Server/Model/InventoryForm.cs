using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class InventoryForm : BaseEntity
    {
        public long RepairOrderId { get; set; }
        public RepairOrder? RepairOrder { get; set; }
        public ICollection<InventoryAccessory>? Accessories { get; set; }
    }
}
