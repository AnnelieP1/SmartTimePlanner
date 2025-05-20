namespace SmartTimePlanner.Models
{
    public class ScheduleItemModel
    {
        public string DayOfWeek { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Activity { get; set; } = null!;
    }
}
