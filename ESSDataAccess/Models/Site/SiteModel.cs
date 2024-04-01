using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using ESSDataAccess.Models.Logs;
using ESSDataAccess.Models.Qr;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Site
{
    public class SiteModel : BaseModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Address { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal Longitude { get; set; }

        public int Radius { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }

        public CommonStatus Status { get; set; } = CommonStatus.Active;

        public bool IsAttendanceEnabled { get; set; } = false;

        public SiteAttendanceTypeEnum? AttendanceType { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        public ClientModel? Client { get; set; }


        [ForeignKey(nameof(DynamicQr))]
        public int? DynamicQrId { get; set; }

        public DynamicQrModel? DynamicQr { get; set; }


        [ForeignKey(nameof(QrGroup))]
        public int? QrGroupId { get; set; }

        public QrCodeGroupModel? QrGroup { get; set; }


        [ForeignKey(nameof(GeofenceGroup))]
        public int? GeofenceGroupId { get; set; }

        public GeofenceGroupModel? GeofenceGroup { get; set; }


        [ForeignKey(nameof(IpGroup))]
        public int? IpGroupId { get; set; }

        public IpGroupModel? IpGroup { get; set; }


        public List<AttendanceModel>? Attendances { get; set; }

        public List<AppUser>? Users { get; set; }

        public List<DynamicQrVerificationLogModel>? DynamicQrVerificationLogs { get; set; }

        public List<QrVerificationLogModel>? QrVerificationLogs { get; set; }

        public List<GeofenceVerificationLogModel>? GeofenceVerificationLogs { get; set; }

        public List<IPVerificationLogModel>? IpVerificationLogs { get; set; }

    }

    public enum SiteAttendanceTypeEnum
    {
        None,
        [Display(Name = "Static QR Code")]
        StaticQRCode,
        [Display(Name = "Dynamic QR Code")]
        DynamicQRCode,
        [Display(Name = "Geo Fence")]
        Geofence,
        [Display(Name = "IP Address")]
        Ip
    }
}
