namespace GarageManagement.Server.dtos
{
    public class BaseResultDto
    {
        public long Id { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
