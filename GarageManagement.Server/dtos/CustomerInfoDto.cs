namespace GarageManagement.Server.dtos
{
    public class CustomerInfoDto
    {
        public string Corporate { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string AlternateMobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DeliveryDate { get; set; }
        public string InsuranceCompany { get; set; } = string.Empty;
    }
}
