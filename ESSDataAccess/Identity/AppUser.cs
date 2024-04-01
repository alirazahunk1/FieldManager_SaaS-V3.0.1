using ESSDataAccess.Enum;
using ESSDataAccess.Models;
using ESSDataAccess.Models.Chat;
using ESSDataAccess.Models.Form;
using ESSDataAccess.Models.Loan;
using ESSDataAccess.Models.Logs;
using ESSDataAccess.Models.Notice;
using ESSDataAccess.Models.Qr;
using ESSDataAccess.Models.Site;
using ESSDataAccess.Models.Task;
using ESSDataAccess.Tenant.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Identity
{
    public class AppUser : IdentityUser<int>
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string? Designation { get; set; }

        public UserGender Gender { get; set; }

        [ForeignKey(nameof(Schedule))]
        public int? ScheduleId { get; set; }
        public ScheduleModel? Schedule { get; set; }

        public int? ParentId { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Active;

        [MaxLength(100)]
        public string? UniqueId { get; set; }

        [MaxLength(250)]
        public string? ImgUrl { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(15)]
        public string? AlternatePhoneNumber { get; set; }

        public DateTime? Dob { get; set; }

        public DateTime? DateOfJoining { get; set; }


        [Column(TypeName = "decimal(10, 2)")]
        public decimal? BaseSalary { get; set; }

        public int? CreatedBy { get; set; }

        [ForeignKey(nameof(Team))]
        public int? TeamId { get; set; }

        public TeamModel? Team { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? PrimarySalesTarget { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? SecondarySalesTarget { get; set; }

        public int AvailableLeaveCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(GeofenceGroup))]
        public int? GeofenceGroupId { get; set; }

        public GeofenceGroupModel? GeofenceGroup { get; set; }

        [ForeignKey(nameof(IpGroup))]
        public int? IpGroupId { get; set; }

        public IpGroupModel? IpGroup { get; set; }

        [ForeignKey(nameof(QrCodeGroup))]

        public int? QrCodeGroupId { get; set; }

        public QrCodeGroupModel? QrCodeGroup { get; set; }


        [ForeignKey(nameof(DynamicQr))]
        public int? DynamicQrId { get; set; }

        public DynamicQrModel? DynamicQr { get; set; }


        [ForeignKey(nameof(Site))]
        public int? SiteId { get; set; }

        public SiteModel? Site { get; set; }


        public EmployeeAttendanceTypeEnum? AttendanceType { get; set; }

        public SalaryTypeEnum SalaryType { get; set; } = SalaryTypeEnum.Monthly;


        [ForeignKey(nameof(Tenant))]
        public int? TenantId { get; set; }
        public TenantModel? Tenant { get; set; }

        // Navigation Properties

        public UserDeviceModel? UserDevice { get; set; }

        public ChatStatusModel? ChatStatus { get; set; }

        public List<ChatModel>? Chats { get; set; }

        public List<ExpenseRequestModel>? ExpenseRequests { get; set; }

        public List<LeaveRequestModel>? LeaveRequests { get; set; }

        public ICollection<AttendanceModel>? Attendances { get; set; }

        public ICollection<ResetPasswordModel>? ResetPasswords { get; set; }

        public PushTokenModel? PushToken { get; set; }

        public List<FormEntryModel>? FormEntries { get; set; }

        public List<FormAssignmentModel>? FormAssignments { get; set; }

        public List<TaskModel>? Tasks { get; set; }

        public List<UserNoticeModel>? UserNotices { get; set; }

        public List<OrderModel>? Orders { get; set; }

        public List<AuditLogModel>? AuditLogs { get; set; }

        public List<LoanRequestModel>? LoanRequests { get; set; }

        public List<SalesTargetModel>? SalesTargets { get; set; }

        public List<PaymentCollectionModel>? PaymentCollections { get; set; }

        public EmployeeSettingsModel? EmployeeSettings { get; set; }


        //Logs
        public List<DynamicQrVerificationLogModel>? DynamicQrVerificationLogs { get; set; }

        public List<GeofenceVerificationLogModel>? GeofenceVerificationLogs { get; set; }

        public List<IPVerificationLogModel>? IpVerificationLogs { get; set; }

        public List<QrVerificationLogModel>? QrVerificationLogs { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
        public string GetFullNameWithCode(string code)
        {
            return code + "_" + Id + " " + GetFullName();
        }

        public void SetIpAttendanceType(int groupId)
        {
            AttendanceType = EmployeeAttendanceTypeEnum.Ip;
            IpGroupId = groupId;
            GeofenceGroupId = null;
            QrCodeGroupId = null;
            DynamicQrId = null;
            SiteId = null;
        }

        public void SetGeofenceAttendanceType(int groupId)
        {
            AttendanceType = EmployeeAttendanceTypeEnum.Geofence;
            GeofenceGroupId = groupId;
            IpGroupId = null;
            QrCodeGroupId = null;
            DynamicQrId = null;
            SiteId = null;
        }

        public void SetQrCodeAttendanceType(int groupId)
        {
            AttendanceType = EmployeeAttendanceTypeEnum.StaticQRCode;
            QrCodeGroupId = groupId;
            GeofenceGroupId = null;
            IpGroupId = null;
            DynamicQrId = null;
            SiteId = null;
        }

        public void SetDynamicQrCodeAttendanceType(int deviceId)
        {
            AttendanceType = EmployeeAttendanceTypeEnum.DynamicQRCode;
            DynamicQrId = deviceId;
            GeofenceGroupId = null;
            IpGroupId = null;
            QrCodeGroupId = null;
            SiteId = null;
        }

        public void SetSiteAttendanceType(int siteId)
        {
            AttendanceType = EmployeeAttendanceTypeEnum.Site;
            SiteId = siteId;
            GeofenceGroupId = null;
            IpGroupId = null;
            QrCodeGroupId = null;
            DynamicQrId = null;
        }

    }

    public enum SalaryTypeEnum
    {
        Monthly,
        Daily,
        Hourly
    }

    public enum EmployeeAttendanceTypeEnum
    {
        None,
        [Display(Name = "Geofence")]
        Geofence,
        [Display(Name = "IP Address")]
        Ip,
        [Display(Name = "Static QR Code")]
        StaticQRCode,
        [Display(Name = "Dynamic QR Code")]
        DynamicQRCode,
        [Display(Name = "Site")]
        Site
    }

}
