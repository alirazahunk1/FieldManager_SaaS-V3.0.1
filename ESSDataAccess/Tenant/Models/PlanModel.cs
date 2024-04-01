using ESSDataAccess.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Tenant.Models
{
    public class PlanModel
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public PlanType Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public PlanStatus Status { get; set; } = PlanStatus.Active;

        public bool IsPromotional { get; set; } = false;

        //Addons

        [Display(Name = "Product & Ordering")]
        public bool IsProductModuleEnabled { get; set; } = false;

        [Display(Name = "Task System")]
        public bool IsTaskModuleEnabled { get; set; } = false;

        [Display(Name = "Notice Board")]
        public bool IsNoticeModuleEnabled { get; set; } = false;

        [Display(Name = "Custom Form")]
        public bool IsDynamicFormModuleEnabled { get; set; } = false;

        [Display(Name = "Expense Management")]
        public bool IsExpenseModuleEnabled { get; set; } = false;

        [Display(Name = "Leave Management")]
        public bool IsLeaveModuleEnabled { get; set; } = false;

        [Display(Name = "Document Management")]
        public bool IsDocumentModuleEnabled { get; set; } = false;

        [Display(Name = "Chat System")]
        public bool IsChatModuleEnabled { get; set; } = false;

        [Display(Name = "Loan Management")]
        public bool IsLoanModuleEnabled { get; set; } = false;

        [Display(Name = "Payment Collection")]
        public bool IsPaymentCollectionModuleEnabled { get; set; } = false;

        [Display(Name = "Geofence")]
        public bool IsGeofenceModuleEnabled { get; set; } = false;

        [Display(Name = "IP Based Attendance")]
        public bool IsIpBasedAttendanceModuleEnabled { get; set; } = false;

        [Display(Name = "UID Based Login")]
        public bool IsUidLoginModuleEnabled { get; set; } = false;

        [Display(Name = "Client Visit")]
        public bool IsClientVisitModuleEnabled { get; set; } = false;

        [Display(Name = "Offline Tracking")]
        public bool IsOfflineTrackingModuleEnabled { get; set; } = false;

        [Display(Name = "Data Import/Export")]
        public bool IsDataImportExportModuleEnabled { get; set; } = false;

        [Display(Name = "Site Management")]
        public bool IsSiteModuleEnabled { get; set; } = false;

        [Display(Name = "QR Code Attendance")]
        public bool IsQrCodeAttendanceModuleEnabled { get; set; } = false;

        [Display(Name = "Dynamic QR Code Attendance")]
        public bool IsDynamicQrCodeAttendanceEnabled { get; set; } = false;

        [Display(Name = "Break Management")]
        public bool IsBreakModuleEnabled { get; set; } = false;

        [Display(Name = "Sales Target")]
        public bool IsSalesTargetModuleEnabled { get; set; } = false;

        [Display(Name = "AI Chat")]
        public bool IsAiChatModuleEnabled { get; set; } = false;


        [Display(Name = "Biometric Verification of In/Out")]
        public bool IsBiometricVerificationModuleEnabled { get; set; } = false;


        public List<TenantSubscriptionModel>? TenantSubscriptions { get; set; }

        public List<OrderModel>? Orders { get; set; }
    }

    public enum PlanStatus
    {
        Active,
        InActive
    }

    public enum PlanType
    {
        Weekly,
        Monthly,
        Yearly
    }
}
