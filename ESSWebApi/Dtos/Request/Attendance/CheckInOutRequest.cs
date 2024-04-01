namespace ESSWebApi.Dtos.Request.Attendance
{
    public class CheckInOutRequest
    {
        public string Status { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool IsMock { get; set; } = false;

        public int? BatteryPercentage { get; set; }

        public bool IsLocationOn { get; set; }

        public bool IsWifiOn { get; set; }

        public int SignalStrength { get; set; }
    }
}
