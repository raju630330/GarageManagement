using System.ComponentModel.DataAnnotations;

namespace GarageManagement.Server.dtos
{
    public class ToBeFilledBySupervisorDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "DriverVoice is required")]
        public string DriverVoice { get; set; } = string.Empty;

        [Required(ErrorMessage = "SupervisorInstructions is required")]
        public string SupervisorInstructions { get; set; } = string.Empty;

        [Required(ErrorMessage = "ActionTaken is required")]
        public string ActionTaken { get; set; } = string.Empty;

        [Required(ErrorMessage = "StartTime is required")]
        public string StartTime { get; set; } = string.Empty;

        [Required(ErrorMessage = "EndTime is required")]
        public string EndTime { get; set; } = string.Empty;
    }
}
