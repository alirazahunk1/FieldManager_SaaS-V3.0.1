using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class UserDeviceModel : BaseModel
    {

        public string DeviceType { get; set; }

        public string DeviceId { get; set; }

        public string Brand { get; set; }

        public string Board { get; set; }

        [MaxLength(10)]
        public string SdkVersion { get; set; }

        public string Model { get; set; }

        public string Token { get; set; } = string.Empty;

        public int BatteryPercentage { get; set; } = 0;

        public bool IsOnline { get; set; } = false;

        public bool IsGPSOn { get; set; } = false;

        public bool IsWifiOn { get; set; } = false;

        [MaxLength(5)]
        public int SignalStrength { get; set; } = 0;

        [Column(TypeName = "decimal(10, 7)")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal? Longitude { get; set; }

        [MaxLength(700)]
        public string? Address { get; set; }

        public bool IsMock { get; set; } = false;

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public AppUser User { get; set; }
    }
}
