namespace ESSWebApi.Dtos.Request.Attendance
{
    public class UpdateStatusRequest
    {
        public string Status { get; set; }

        public int Accuracy { get; set; }

        public string Activity { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool IsMock { get; set; } = false;

        public int? BatteryPercentage { get; set; }

        public bool IsGPSOn { get; set; }

        public bool IsWifiOn { get; set; }

        public int SignalStrength { get; set; }
    }
}
