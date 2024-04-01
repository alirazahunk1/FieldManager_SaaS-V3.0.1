using ESSDataAccess.Models.Base;

namespace ESSDataAccess.Models.Addons
{
    public class AddonSettingModel : NoTenantBaseModel
    {
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
    }
}
