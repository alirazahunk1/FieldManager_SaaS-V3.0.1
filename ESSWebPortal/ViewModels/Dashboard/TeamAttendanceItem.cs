namespace ESSWebPortal.ViewModels.Dashboard
{
    public class TeamAttendanceItem
    {
        public string? Name { get; set; }

        public int Present { get; set; } = 0;

        public int Absent { get; set; } = 0;

        public int OnLeave { get; set; } = 0;
    }
}
