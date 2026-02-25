using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.dtos
{
    public class AdditionalJobObserveDetailDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "TechnicianVoice is required")]
        public string TechnicianVoice { get; set; } = string.Empty;

        [Required(ErrorMessage = "SupervisorInstructions is required")]
        public string SupervisorInstructions { get; set; } = string.Empty;

        [Required(ErrorMessage = "ActionTaken is required")]
        public string ActionTaken { get; set; } = string.Empty;

        [Required(ErrorMessage = "StartTime is required")]
        public string StartTime { get; set; } = string.Empty;

        [Required(ErrorMessage = "EndTime is required")]
        public string EndTime { get; set; } = string.Empty;
        public long RepairOrderId { get; set; }
    }
}
