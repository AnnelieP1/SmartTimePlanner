using System.ComponentModel.DataAnnotations;

namespace SmartTimePlanner.Models
{
    public class ScheduleInputModel
    {
        [Required(ErrorMessage = "Consultant is required.")]
        public string ConsultantName { get; set; } = null!;

        [Required]
        public string DayOfWeek { get; set; } = null!;

        [Required(ErrorMessage = "Start time is required.")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Activity is required.")]
        public string Activity { get; set; } = null!;

        
    }
}
