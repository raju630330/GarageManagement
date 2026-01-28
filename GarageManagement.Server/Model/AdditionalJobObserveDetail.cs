using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.Model
{
    public class AdditionalJobObserveDetail
    {
        public long Id { get; set; }
        public string TechnicianVoice { get; set; } = string.Empty;
        public string SupervisorInstructions { get; set; } = string.Empty;
        public string ActionTaken { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public long? RepairOrderId { get; set; }
        public RepairOrder RepairOrder { get; set; }
    }

}
