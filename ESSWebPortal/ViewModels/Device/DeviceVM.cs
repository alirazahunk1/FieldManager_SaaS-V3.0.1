using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.ViewModels.Device
{
    public class DeviceVM
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Device Type")]
        public string DeviceType { get; set; }

        [Display(Name = "Device Id")]
        public string DeviceId { get; set; }

        public string Brand { get; set; }

        public string Board { get; set; }

        [Display(Name = "Sdk Version")]
        public string SdkVersion { get; set; }

        public string Model { get; set; }

        public string Token { get; set; } = string.Empty;

        public int BatteryPercentage { get; set; } = 0;

        public bool IsOnline { get; set; } = false;

        public bool IsGPSOn { get; set; } = false;

        public bool IsWifiOn { get; set; } = false;

        public int SignalStrength { get; set; } = 0;

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string? Address { get; set; }

        public bool IsMock { get; set; } = false;

        [Display(Name = "Updated On")]
        public DateTime LastUpdatedOn { get; set; }
    }
}
