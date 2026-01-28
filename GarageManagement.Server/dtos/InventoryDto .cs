namespace GarageManagement.Server.dtos
{
    public class InventoryDto
    {
        public long Id { get; set; }
        public List<AccessoryDto> Accessories { get; set; }
        public long RepairOrderId { get; set; }

    }
    public class AccessoryDto
    {
        public string Label { get; set; }
        public bool Checked { get; set; }   
    }
}
