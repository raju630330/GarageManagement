using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class InventoryForm
    {
        [Key]
        public int Id { get; set; }

        public ICollection<InventoryAccessories> Accessories { get; set; }
    }
}
