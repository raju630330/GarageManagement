using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class InventoryForm
    {
        public long Id { get; set; }

        public ICollection<InventoryAccessory> Accessories { get; set; }
    }
}
