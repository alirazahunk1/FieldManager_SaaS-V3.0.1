

using ESSCommon.Core.SharedModels;
using ESSCommon.Core.SharedModels.Settings;
using ESSDataAccess.Models;
using ESSWebPortal.Core.ViewModel.SuperAdmin;

namespace ESSWebPortal.Core.SuperAdmin
{
    public interface ISASettings
    {
        Task<PaymentGateway> GetDefaultPaymentGateway();

        Task<RazorPaySettingsDto> GetRazorPaySettings();

        Task<bool> UpdateRazorpaySettings(string razorPayKeyId, string razorPayKeySecret, int? userId = null);

        Task<bool> ChangeDefaultPaymentGateway(PaymentGateway gateway, int? userId = null);

        Task AddSettings(SASettingsVM settings, int userId);

        Task<bool> UpdateTwilioSettings(string sid, string authToken, string fromNumber, int? userId = null);

        Task<bool> UpdateTwilioStatus(bool status, int? userId = null);

        Task<bool> UpdateBasicSettings(string appName, string appVersion, string country, string phoneCountryCode, string currency, string currencySymbol, int? userId = null);

        /*Task<bool> UpdateFireBaseSettings(string fCMServerKey, string fCMSenderId, int? userId = null);*/

        Task<bool> UpdateDashboardSettings(string offlineCheckTimeType, string offlineCheckTime, int? userId = null);

        Task<bool> UpdateMobileAppSettings(string appVersion, string mLocationUpdateIntervalType, string mLocationUpdateInterval, string mPrivacyPolicyLink, string ApiBaseUrl, string webBaseUrl, int? userId = null);

        Task<bool> UpdatePaypalSettings(string paypalClientId, string paypalClientSecret, PaypalModeEnum paypalMode, int? userId = null);

        Task<string> GetOfflineCheckType();

        Task<string> GetCurrencySymbol();

        Task<string> GetCurrency();

        Task<string> GetAppVersion();

        Task<string> GetAppName();

        Task<string> GetApiBaseUrl();

        Task<int> GetOfflineCheckTime();

        Task<SASettingsVM> GetAll();

        Task<PaypalSettingsVM> GetPaypalSettings();

        Task<ModuleSettingsDto> GetModuleSettings();

        //Modules
        Task<bool> ChangeLeaveModuleStatus();

        Task<bool> ChangeExpenseModuleStatus();

        Task<bool> ChangeDocumentModuleStatus();

        Task<bool> ChangeTaskModuleStatus();

        Task<bool> ChangeNoticeModuleStatus();

        Task<bool> ChangeFormModuleStatus();

        Task<bool> ChangeProductModuleStatus();

        Task<bool> ChangeChatModuleStatus();

        Task<bool> ChangeLoanModuleStatus();

        Task<bool> ChangeAiChatModuleStatus();

        Task<bool> ChangePaymentCollectionModuleStatus();


        Task<bool> ChangeDynamicQrCodeAttendanceStatus();

        Task<bool> ChangeBiometricVerificationStatus();


        Task<bool> ChangeGeofenceModuleStatus();

        Task<bool> ChangeIpBasedAttendanceModuleStatus();

        Task<bool> ChangeUidLoginModuleStatus();

        Task<bool> ChangeClientVisitModuleStatus();

        Task<bool> ChangeDataImportExportModuleStatus();

        Task<bool> ChangeSiteModuleStatus();

        Task<bool> ChangeQrCodeAttendanceModuleStatus();

        Task<bool> ChangeBreakModuleStatus();

        Task<bool> ChangeOfflineTrackingModuleStatus();
    }
}
