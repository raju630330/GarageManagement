namespace GarageManagement.Server.dtos
{
    public class InventoryDto
    {
        public List<AccessoryDto> Accessories { get; set; }

    }
    public class AccessoryDto
    {
        public string Label { get; set; }
        public bool Checked { get; set; }   
    }
}
