namespace GarageManagement.Server.Model
{
    public class ToBeFilledBySupervisor
    {
        public int Id { get; set; } 
        public string DriverVoice { get; set; } = string.Empty;
        public string SupervisorInstructions { get; set; } = string.Empty;
        public string ActionTaken { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
    }
}