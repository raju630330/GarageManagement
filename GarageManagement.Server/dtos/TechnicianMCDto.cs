namespace GarageManagement.Server.dtos
{
    public class TechnicianMCDto
    {
        public long Id { get; set; }
        public long? RepairOrderId { get; set; }

        public string? Remarks { get; set; }
        public string? TechnicianSign { get; set; }
        public string? DriverSign { get; set; }
        public string? FloorSign { get; set; }
        public string? AuthSign { get; set; }

        public List<CheckItemDto>? CheckList { get; set; }
    }
    public class CheckItemDto
    {
        public long Id { get; set; }
        public string? Label { get; set; }
        public string? Control { get; set; }
        public string? Status { get; set; }
    }

}
