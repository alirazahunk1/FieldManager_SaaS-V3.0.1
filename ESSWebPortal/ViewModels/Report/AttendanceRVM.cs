namespace ESSWebPortal.ViewModels.Report
{
    public class AttendanceRVM
    {
        public string Name { get; set; }

        public List<AttendanceReportItem> Items { get; set; } = new List<AttendanceReportItem>();
    }

    public partial class AttendanceReportItem
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public bool IsPresent { get; set; }

        public string InTime { get; set; }

        public string OutTime { get; set; }

        public string WorkingHours { get; set; }

        public string Remarks { get; set; }
    }
}
