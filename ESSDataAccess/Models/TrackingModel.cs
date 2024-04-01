using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class TrackingModel : BaseModel
    {
        [ForeignKey(nameof(Attendance))]
        public int AttendanceId { get; set; }

        public AttendanceModel? Attendance { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal Longitude { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public double? Altitude { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public double? Bearing { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal? Speed { get; set; }

        [MaxLength(700)]
        public string? Address { get; set; }

        public bool IsMock { get; set; } = false;

        public int BatteryPercentage { get; set; } = 0;

        public bool IsGPSOn { get; set; } = true;

        public bool IsWifiOn { get; set; } = false;

        [MaxLength(5)]
        public int SignalStrength { get; set; } = 1;

        public int? Accuracy { get; set; }

        [MaxLength(50)]
        public string? Activity { get; set; }

        [MaxLength(1000)]
        public string? ImageUrl { get; set; }

        public TrackingTypeEnum Type { get; set; }

        public bool IsOffline { get; set; } = false;

        public DateTime? OfflineSyncedAt { get; set; }
    }


    public enum TrackingTypeEnum
    {
        CheckedIn,
        Travelling,
        Still,
        CheckedOut,
        AutoCheckedOut,
        ProofPost
    }
}
