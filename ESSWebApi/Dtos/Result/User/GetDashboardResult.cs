
namespace ESSWebApi.Dtos.Result.User
{
    public class GetDashboardResult
    {
        public string ScheduleTime { get; set; }

        public List<bool> ScheduleWeeks { get; set; }

        public int PresentDays { get; set; } = 0;

        public int HalfDays { get; set; } = 0;

        public int AbsentDays { get; set; } = 0;

        public int WeekOffDays { get; set; } = 0;

        public int OnLeaveDays { get; set; } = 0;

        public int AvailableLeaveCount { get; set; } = 0;


        public double Travelled { get; set; } = 0.0;


        public decimal Approved { get; set; } = 0;

        public decimal Pending { get; set; } = 0;

        public decimal Rejected { get; set; } = 0;
    }
}
