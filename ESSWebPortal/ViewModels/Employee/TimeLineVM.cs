using ESSDataAccess.Models;

namespace ESSWebPortal.ViewModels.Employee
{
    public class TimeLineVM
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int AttendanceId { get; set; }

        public string TotalTrackedTime { get; set; }

        public string TotalAttendanceTime { get; set; }

        public string DeviceInfo { get; set; }

        public double TotalKM { get; set; }

        public List<TimeLineItemVM> TimeLineItems { get; set; }
    }

    public partial class TimeLineItemVM
    {
        public int Id { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string? Address { get; set; }

        public string Activity { get; set; }

        public int Accuracy { get; set; }

        public int BatteryPercentage { get; set; }

        public int SignalStrength { get; set; }

        public bool IsGPSOn { get; set; }

        public bool IsWifiOn { get; set; }

        public TrackingTypeEnum TrackingType { get; set; }

        public string Type { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string ElapseTime { get; set; }

        public double Distance { get; set; } = 0;
    }
}
