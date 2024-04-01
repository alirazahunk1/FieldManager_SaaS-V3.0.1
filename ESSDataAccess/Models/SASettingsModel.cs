using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models
{
    public class SASettingsModel : NoTenantBaseModel
    {
        //Basic Settings
        public string AppName { get; set; }

        public string AppVersion { get; set; }

        [StringLength(20)]
        public string Country { get; set; }

        [StringLength(4)]
        public string PhoneCountryCode { get; set; }

        [StringLength(15)]
        public string Currency { get; set; }

        [MaxLength(4)]
        public string CurrencySymbol { get; set; }


        //Mobile App Settings
        public string MAppVersion { get; set; }

        [MaxLength(10)]
        public string MLocationUpdateIntervalType { get; set; }

        public int MLocationUpdateInterval { get; set; }

        [MaxLength(100)]
        public string MPrivacyPolicyLink { get; set; }

        [MaxLength(200)]
        public string ApiBaseUrl { get; set; }

        [MaxLength(200)]
        public string WebBaseUrl { get; set; }


        //SMS settings
        public bool TwilioStatus { get; set; }

        [MaxLength(200)]
        public string TwilioAccountSid { get; set; }

        [MaxLength(400)]
        public string TwilioAuthToken { get; set; }

        [MaxLength(30)]
        public string TwilioFromNumber { get; set; }

        //Dashboard Settings
        [MaxLength(10)]
        public string OfflineCheckTimeType { get; set; }

        public int OfflineCheckTime { get; set; }

        //Paypal Settings

        [MaxLength(500)]
        public string? PaypalClientId { get; set; }

        [MaxLength(500)]
        public string? PaypalClientSecret { get; set; }

        public PaypalModeEnum? PaypalMode { get; set; }

        [MaxLength(1000)]
        public string LeaveDocumentPath { get; set; }

        [MaxLength(1000)]
        public string PaySlipPath { get; set; }

        [MaxLength(1000)]
        public string ExpenseDocumentPath { get; set; }

        public PaymentGateway DefaultPaymentGateway { get; set; } = PaymentGateway.Paypal;

        [MaxLength(500)]
        public string? RazorPayKeyId { get; set; }

        [MaxLength(500)]
        public string? RazorPayKeySecret { get; set; }

        public int BillingCycleDate { get; set; }

        public int DueDays { get; set; }

        public int OverDueDays { get; set; }

        [MaxLength(10)]
        public string? InvoicePrefix { get; set; }

        //Modules

        public bool IsProductModuleEnabled { get; set; }

        public bool IsTaskModuleEnabled { get; set; }

        public bool IsNoticeModuleEnabled { get; set; }

        public bool IsDynamicFormModuleEnabled { get; set; }

        public bool IsExpenseModuleEnabled { get; set; }

        public bool IsLeaveModuleEnabled { get; set; }

        public bool IsDocumentModuleEnabled { get; set; }

        public bool IsChatModuleEnabled { get; set; }

        public bool IsLoanModuleEnabled { get; set; }


        public bool IsPaymentCollectionModuleEnabled { get; set; }

        public bool IsGeofenceModuleEnabled { get; set; }

        public bool IsIpBasedAttendanceModuleEnabled { get; set; }

        public bool IsUidLoginModuleEnabled { get; set; }

        public bool IsClientVisitModuleEnabled { get; set; }

        public bool IsOfflineTrackingModuleEnabled { get; set; }

        public bool IsDataImportExportModuleEnabled { get; set; }

        public bool IsSiteModuleEnabled { get; set; }

        public bool IsQrCodeAttendanceModuleEnabled { get; set; }

        public bool IsDynamicQrCodeAttendanceEnabled { get; set; }

        public bool IsBreakModuleEnabled { get; set; }

        public bool IsSalesTargetModuleEnabled { get; set; }

        public bool IsAiChatModuleEnabled { get; set; }

        public bool IsBiometricVerificationRequired { get; set; }

    }

    public enum PaypalModeEnum
    {
        Sandbox,
        Live
    }

    public enum PaymentGateway
    {
        Paypal,
        RazorPay,
        Manual
    }
}
