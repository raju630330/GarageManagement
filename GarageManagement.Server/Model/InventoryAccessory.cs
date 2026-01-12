using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class InventoryAccessory
    {
        public long Id { get; set; }
        public string Label { get; set; }
        public bool Checked { get; set; }

        public long InventoryFormId { get; set; }
        public InventoryForm InventoryForm { get; set; }
        public long? RepairOrderId { get; set; }
        public RepairOrder RepairOrder { get; set; }
    }
}
