namespace SmartTimePlanner.Models
{
    public class ConsultantScheduleModel
    {
        public string ConsultantName { get; set; } = null!;
        public List<ScheduleItemModel> ScheduleItems { get; set; } = new();
    }
}
