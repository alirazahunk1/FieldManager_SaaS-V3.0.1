

namespace ESSWebApi.Dtos.Request.Device
{
    public class DeviceStatusUpdateRequest
    {
        public int BatteryPercentage { get; set; }

        public bool IsGPSOn { get; set; }
        
        public bool IsWifiOn { get; set; }

        public int SignalStrength { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool IsMock { get; set; } = false;
    }
}
