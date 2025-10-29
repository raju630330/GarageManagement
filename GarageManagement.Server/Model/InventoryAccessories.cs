using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class InventoryAccessories
    {
        [Key]
        public int Id { get; set; }
        public string Label { get; set; }
        public bool Checked { get; set; }

        public int InventoryFormId { get; set; }
        public InventoryForm InventoryForm { get; set; }
    }
}
