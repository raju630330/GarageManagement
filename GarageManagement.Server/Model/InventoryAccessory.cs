using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class InventoryAccessory : BaseEntity
    {
        public string Label { get; set; } = string.Empty;
        public bool Checked { get; set; }
        public long InventoryFormId { get; set; }
        public InventoryForm? InventoryForm { get; set; }
    }
}
