namespace GarageManagement.Server.Model
{
    public class JobCardTyreBattery : BaseEntity
    {
        public long JobCardId { get; set; }
        public JobCard JobCard { get; set; } = null!;
        public string Type { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Condition { get; set; } = string.Empty;
    }
}
