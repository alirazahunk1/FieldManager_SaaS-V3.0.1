using ESSDataAccess.Enum;

namespace ESSWebPortal.ViewModels.Schedule
{
    public class ScheduleVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public ScheduleStatus Status { get; set; }

        public bool Sunday { get; set; }

        public bool Monday { get; set; }

        public bool Tuesday { get; set; }

        public bool Wednesday { get; set; }

        public bool Thursday { get; set; }

        public bool Friday { get; set; }

        public bool Saturday { get; set; }
    }
}
