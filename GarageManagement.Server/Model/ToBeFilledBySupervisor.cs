namespace GarageManagement.Server.Model
{
    public class ToBeFilledBySupervisor
    {
        public int Id { get; set; }
        public string DriverVoice { get; set; }
        public string SupervisorInstruction { get; set; }
        public string ActionTaken { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}