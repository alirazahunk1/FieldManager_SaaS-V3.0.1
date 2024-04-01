using ESSDataAccess.Tenant.Models;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.Core.ViewModel.Plan
{
    public class PlanCreateUpdateVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Price per employee")]
        public decimal Price { get; set; }

        public PlanType Type { get; set; }

        [Display(Name = "Promotional Plan")]
        public bool IsPromotional { get; set; }

        [Display(Name = "Biometric Verification for Check In/Out")]
        public bool IsBiometricVerificationModuleEnabled { get; set; }

        [Display(Name = "Product & Ordering")]
        public bool IsProductModuleEnabled { get; set; }

        [Display(Name = "Task Management")]
        public bool IsTaskModuleEnabled { get; set; }

        [Display(Name = "Notice Board")]
        public bool IsNoticeModuleEnabled { get; set; }

        [Display(Name = "Custom Form")]
        public bool IsDynamicFormModuleEnabled { get; set; }

        [Display(Name = "Expense Management")]
        public bool IsExpenseModuleEnabled { get; set; }

        [Display(Name = "Leave Management")]
        public bool IsLeaveModuleEnabled { get; set; }

        [Display(Name = "Document Request")]
        public bool IsDocumentModuleEnabled { get; set; }

        [Display(Name = "Chat System")]
        public bool IsChatModuleEnabled { get; set; }

        [Display(Name = "Loan Management")]
        public bool IsLoanModuleEnabled { get; set; }

        [Display(Name = "Payment Collection")]
        public bool IsPaymentCollectionModuleEnabled { get; set; }

        [Display(Name = "Geofence Attendance")]
        public bool IsGeofenceModuleEnabled { get; set; }

        [Display(Name = "IP Based Attendance")]
        public bool IsIpBasedAttendanceModuleEnabled { get; set; }

        [Display(Name = "UID Login")]
        public bool IsUidLoginModuleEnabled { get; set; }

        [Display(Name = "Client Visit")]
        public bool IsClientVisitModuleEnabled { get; set; }

        [Display(Name = "Offline Tracking")]
        public bool IsOfflineTrackingModuleEnabled { get; set; }

        [Display(Name = "Data Import/Export")]
        public bool IsDataImportExportModuleEnabled { get; set; }

        [Display(Name = "Site Management")]
        public bool IsSiteModuleEnabled { get; set; }

        [Display(Name = "Qr Code Attendance")]
        public bool IsQrCodeAttendanceModuleEnabled { get; set; }

        [Display(Name = "Dynamic Qr Code Attendance")]
        public bool IsDynamicQrCodeAttendanceEnabled { get; set; }

        [Display(Name = "Break System")]
        public bool IsBreakModuleEnabled { get; set; }

        [Display(Name = "Sales Target")]
        public bool IsSalesTargetModuleEnabled { get; set; }

        [Display(Name = "Ai Chat")]
        public bool IsAiChatModuleEnabled { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
