using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using ESSDataAccess.Models.Logs;
using ESSDataAccess.Models.Site;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models.Qr
{
    public class DynamicQrModel : BaseModel
    {
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        [MaxLength(12)]
        public string? UniqueId { get; set; }

        [MaxLength(6)]
        public string? Pin { get; set; }

        [MaxLength(250)]
        public string? QrValue { get; set; }

        [MaxLength(500)]
        public string? Token { get; set; }

        public DateTime? QrLastUpdatedAt { get; set; }

        public int? QrUpdateInterval { get; set; }

        public DateTime? QrExpiryDate { get; set; }

        public DynamicQrStatus Status { get; set; } = DynamicQrStatus.New;

        public QrDeviceType DeviceType { get; set; }

        public SiteModel? Site { get; set; }

        public AppUser? User { get; set; }

        public List<DynamicQrVerificationLogModel>? DynamicQrVerificationLogs { get; set; }

    }

    public enum QrDeviceType
    {
        Android,
        Ios,
        Windows,
        Mac,
        Linux,
        Web,
        Other,
    }

    public enum DynamicQrStatus
    {
        New,
        InUse,
        Deactivated
    }
}
