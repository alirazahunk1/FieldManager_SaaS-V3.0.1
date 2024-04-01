using AutoMapper;
using ESSCommon.Core.SharedModels;
using ESSCommon.Core.SharedModels.Settings;
using ESSDataAccess.DbContext;
using ESSDataAccess.Models;
using ESSWebPortal.Core.ViewModel.SuperAdmin;
using Microsoft.EntityFrameworkCore;

namespace ESSWebPortal.Core.SuperAdmin
{
    public class SASettingsService : ISASettings
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SASettingsService(AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddSettings(SASettingsVM settings, int userId)
        {
            var setting = _mapper.Map<SASettingsVM, SettingsModel>(settings);

            setting.CreatedAt = setting.UpdatedAt = DateTime.Now;
            setting.UpdatedBy = setting.CreatedBy = userId;

            await _context.AppSettings.AddAsync(setting);
            await _context.SaveChangesAsync();
        }


        public async Task<string> GetOfflineCheckType()
        {
            var data = await _context.SASettings
                .Select(x => x.OfflineCheckTimeType)
                .FirstOrDefaultAsync();

            if (data == null || string.IsNullOrEmpty(data)) return "m";

            return data.ToLower().Equals("minutes") ? "m" : "s";

        }

        public async Task<int> GetOfflineCheckTime()
        {
            var data = await _context.SASettings
                .Select(x => x.OfflineCheckTime)
                .FirstOrDefaultAsync();

            if (data == 0) return 30;

            return data;
        }

        public async Task<SASettingsVM> GetAll()
        {
            SASettingsVM vm = new SASettingsVM();

            var settings = await _context.SASettings
                .AsNoTracking()
                .FirstOrDefaultAsync();
            //Basic
            vm.AppName = settings.AppName;
            vm.Country = settings.Country;
            vm.PhoneCountryCode = settings.PhoneCountryCode;
            vm.AppVersion = settings.AppVersion;
            vm.Currency = settings.Currency;
            vm.CurrencySymbol = settings.CurrencySymbol;

            //Mobile
            vm.MAppVersion = settings.MAppVersion;
            vm.MPrivacyPolicyLink = settings.MPrivacyPolicyLink;
            vm.MLocationUpdateInterval = settings.MLocationUpdateInterval;
            vm.MLocationUpdateIntervalType = settings.MLocationUpdateIntervalType;
            vm.ApiBaseUrl = settings.ApiBaseUrl;
            vm.WebBaseUrl = settings.WebBaseUrl;

            //PayPal
            vm.PaypalClientId = settings.PaypalClientId;
            vm.PaypalClientSecret = settings.PaypalClientSecret;
            vm.PaypalMode = settings.PaypalMode;

            //Twilio
            vm.TwilioIsEnabled = settings.TwilioStatus;
            vm.TwilioFromNumber = settings.TwilioFromNumber;
            vm.TwilioAccountSid = settings.TwilioAccountSid;
            vm.TwilioAuthToken = settings.TwilioAuthToken;

            //Dashboard
            vm.OfflineCheckTime = settings.OfflineCheckTime;
            vm.OfflineCheckTimeType = settings.OfflineCheckTimeType;

            //Razor Pay
            vm.RazorPayKeyId = settings.RazorPayKeyId;
            vm.RazorPayKeySecret = settings.RazorPayKeySecret;

            vm.PaymentGateway = settings.DefaultPaymentGateway;


            return vm;
        }

        public async Task<bool> UpdateSettings(SASettingsVM settings, int userId)
        {
            try
            {
                var setting = _mapper.Map<SASettingsVM, SettingsModel>(settings);

                setting.CreatedAt = setting.UpdatedAt = DateTime.Now;
                setting.UpdatedBy = setting.CreatedBy = userId;

                await _context.AppSettings.AddAsync(setting);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateTwilioSettings(string sid, string authToken, string fromNumber, int? userId = null)
        {
            if (string.IsNullOrWhiteSpace(sid) || string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(fromNumber)) { return false; }
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();
                settings.TwilioAccountSid = sid;
                settings.TwilioAuthToken = authToken;
                settings.TwilioFromNumber = fromNumber;
                settings.UpdatedAt = DateTime.Now;
                settings.UpdatedBy = userId;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateTwilioStatus(bool status, int? userId = null)
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();
                settings.TwilioStatus = status;
                settings.UpdatedAt = DateTime.Now;
                settings.UpdatedBy = userId;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateBasicSettings(string appName, string appVersion, string country, string phoneCountryCode, string currency, string currencySymbol, int? userId = null)
        {
            if (string.IsNullOrEmpty(appName) || string.IsNullOrEmpty(appVersion) ||
                string.IsNullOrEmpty(country) || string.IsNullOrEmpty(phoneCountryCode) ||
                string.IsNullOrEmpty(currency) || string.IsNullOrEmpty(currencySymbol)) { return false; }

            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.AppName = appName;
                settings.AppVersion = appVersion;
                settings.Country = country;
                settings.PhoneCountryCode = phoneCountryCode;
                settings.Currency = currency;
                settings.CurrencySymbol = currencySymbol;
                settings.UpdatedAt = DateTime.Now;
                settings.UpdatedBy = userId;

                _context.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /*public async Task<bool> UpdateFireBaseSettings(string fCMServerKey, string fCMSenderId, int? userId = null)
        {
            if (string.IsNullOrEmpty(fCMServerKey) || string.IsNullOrEmpty(fCMSenderId)) { return false; }

            try
            {
                var settings = await _context.AppSettings.FirstOrDefaultAsync();

                settings.FCMServerKey = fCMServerKey;
                settings.FCMSenderId = fCMSenderId;
                settings.UpdatedAt = DateTime.Now;
                settings.UpdatedBy = userId;

                _context.AppSettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }*/

        public async Task<bool> UpdateDashboardSettings(string offlineCheckTimeType, string offlineCheckTime, int? userId = null)
        {
            if (string.IsNullOrEmpty(offlineCheckTimeType) || string.IsNullOrEmpty(offlineCheckTime)) { return false; }

            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.OfflineCheckTimeType = offlineCheckTimeType;
                settings.OfflineCheckTime = Convert.ToInt32(offlineCheckTime);
                settings.UpdatedAt = DateTime.Now;
                settings.UpdatedBy = userId;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> UpdateMobileAppSettings(string appVersion, string mLocationUpdateIntervalType, string mLocationUpdateInterval, string mPrivacyPolicyLink, string ApiBaseUrl, string webBaseUrl, int? userId = null)
        {
            if (string.IsNullOrEmpty(appVersion) || string.IsNullOrEmpty(mPrivacyPolicyLink) || string.IsNullOrEmpty(ApiBaseUrl) || string.IsNullOrEmpty(webBaseUrl)) { return false; }

            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.MLocationUpdateInterval = Convert.ToInt32(mLocationUpdateInterval);
                settings.MLocationUpdateIntervalType = mLocationUpdateIntervalType;
                settings.MPrivacyPolicyLink = mPrivacyPolicyLink;
                settings.MAppVersion = appVersion;
                settings.ApiBaseUrl = ApiBaseUrl;
                settings.WebBaseUrl = webBaseUrl;
                settings.UpdatedAt = DateTime.Now;
                settings.UpdatedBy = userId;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception) { return false; }
        }

        public async Task<bool> UpdatePaypalSettings(string paypalClientId, string paypalClientSecret, PaypalModeEnum paypalMode, int? userId = null)
        {
            if (string.IsNullOrEmpty(paypalClientId) || string.IsNullOrEmpty(paypalClientSecret)) { return false; }

            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.PaypalClientId = paypalClientId;
                settings.PaypalClientSecret = paypalClientSecret;
                settings.PaypalMode = paypalMode;
                settings.UpdatedAt = DateTime.Now;
                settings.UpdatedBy = userId;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception) { return false; }
        }

        public async Task<string> GetCurrencySymbol()
        {
            var currency = await _context.SASettings
                 .Select(x => x.CurrencySymbol)
                 .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(currency)) return "$";

            return currency;
        }
        public async Task<string> GetCurrency()
        {
            var currency = await _context.SASettings
                 .Select(x => x.Currency)
                 .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(currency)) return "USD";

            return currency;
        }

        public async Task<string> GetAppVersion()
        {
            var appVersion = await _context.SASettings
                .Select(x => x.AppVersion)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(appVersion)) return "1.0";

            return appVersion;
        }

        public async Task<string> GetAppName()
        {
            var appName = await _context.SASettings
                .Select(x => x.AppName)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(appName)) return "Field Manager";

            return appName;
        }

        public async Task<string> GetApiBaseUrl()
        {
            var data = await _context.SASettings
               .Select(x => x.ApiBaseUrl)
               .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(data)) return "";

            return data;
        }

        public async Task<PaypalSettingsVM> GetPaypalSettings()
        {
            var paypalSettings = await _context.SASettings
                .Select(x => new PaypalSettingsVM
                {
                    PaypalMode = x.PaypalMode,
                    PaypalClientId = x.PaypalClientId,
                    PaypalClientSecret = x.PaypalClientSecret
                }).FirstOrDefaultAsync();
            return paypalSettings;
        }

        public async Task<PaymentGateway> GetDefaultPaymentGateway()
        {
            return await _context.SASettings.Select(x => x.DefaultPaymentGateway).FirstOrDefaultAsync();
        }

        public async Task<RazorPaySettingsDto> GetRazorPaySettings()
        {
            return await _context.SASettings
                .Select(x => new RazorPaySettingsDto
                {
                    RazorPayKeyId = x.RazorPayKeyId,
                    RazorPayKeySecret = x.RazorPayKeySecret,
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateRazorpaySettings(string razorPayKeyId, string razorPayKeySecret, int? userId = null)
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();
                if (settings != null)
                {
                    settings.RazorPayKeyId = razorPayKeyId;
                    settings.RazorPayKeySecret = razorPayKeySecret;
                    settings.UpdatedAt = DateTime.Now;
                    _context.Update(settings);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ChangeDefaultPaymentGateway(PaymentGateway gateway, int? userId = null)
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();
                if (settings != null)
                {
                    settings.DefaultPaymentGateway = gateway;
                    settings.UpdatedAt = DateTime.Now;
                    _context.Update(settings);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public async Task<bool> ChangeLeaveModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsLeaveModuleEnabled = !settings.IsLeaveModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeExpenseModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsExpenseModuleEnabled = !settings.IsExpenseModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeDocumentModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsDocumentModuleEnabled = !settings.IsDocumentModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeTaskModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsTaskModuleEnabled = !settings.IsTaskModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeNoticeModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsNoticeModuleEnabled = !settings.IsNoticeModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeFormModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsDynamicFormModuleEnabled = !settings.IsDynamicFormModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeProductModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsProductModuleEnabled = !settings.IsProductModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeChatModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsChatModuleEnabled = !settings.IsChatModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> GetLeaveModuleStatus()
        {

            var settings = await _context.SASettings.FirstOrDefaultAsync();

            return settings.IsLeaveModuleEnabled;
        }

        public async Task<bool> GetExpenseModuleStatus()
        {
            var settings = await _context.SASettings.FirstOrDefaultAsync();

            return settings.IsExpenseModuleEnabled;
        }

        public async Task<ModuleSettingsDto> GetModuleSettings()
        {
            var settings = await _context.SASettings
                .Select(x => new ModuleSettingsDto
                {
                    IsChatModuleEnabled = x.IsChatModuleEnabled,
                    IsDocumentModuleEnabled = x.IsDocumentModuleEnabled,
                    IsDynamicFormModuleEnabled = x.IsDynamicFormModuleEnabled,
                    IsExpenseModuleEnabled = x.IsExpenseModuleEnabled,
                    IsLeaveModuleEnabled = x.IsLeaveModuleEnabled,
                    IsNoticeModuleEnabled = x.IsNoticeModuleEnabled,
                    IsProductModuleEnabled = x.IsProductModuleEnabled,
                    IsTaskModuleEnabled = x.IsTaskModuleEnabled,
                    IsLoanModuleEnabled = x.IsLoanModuleEnabled,
                    IsAiChatModuleEnabled = x.IsAiChatModuleEnabled,
                    IsPaymentCollectionModuleEnabled = x.IsPaymentCollectionModuleEnabled,
                    IsOfflineTrackingModuleEnabled = x.IsOfflineTrackingModuleEnabled,
                    IsGeofenceModuleEnabled = x.IsGeofenceModuleEnabled,
                    IsDynamicQrCodeAttendanceEnabled = x.IsDynamicQrCodeAttendanceEnabled,
                    IsIpBasedAttendanceModuleEnabled = x.IsIpBasedAttendanceModuleEnabled,
                    IsBreakModuleEnabled = x.IsBreakModuleEnabled,
                    IsClientVisitModuleEnabled = x.IsClientVisitModuleEnabled,
                    IsSiteModuleEnabled = x.IsSiteModuleEnabled,
                    IsUidLoginModuleEnabled = x.IsUidLoginModuleEnabled,
                    IsQrCodeAttendanceModuleEnabled = x.IsQrCodeAttendanceModuleEnabled,
                    IsDataImportExportModuleEnabled = x.IsDataImportExportModuleEnabled,
                    IsBiometricVerificationModuleEnabled = x.IsBiometricVerificationRequired
                }).FirstOrDefaultAsync();

            return settings;
        }

        public async Task<string> GetOrderPrefix()
        {
            var settings = await _context.AppSettings.FirstOrDefaultAsync();

            return settings.OrderPrefix;
        }

        public async Task<bool> ChangeLoanModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsLoanModuleEnabled = !settings.IsLoanModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> GetLoanModuleStatus()
        {
            var settings = await _context.SASettings.FirstOrDefaultAsync();

            return settings.IsLoanModuleEnabled;
        }

        public async Task<bool> ChangeAiChatModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsAiChatModuleEnabled = !settings.IsAiChatModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangePaymentCollectionModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsPaymentCollectionModuleEnabled = !settings.IsPaymentCollectionModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> GetAiChatModuleStatus()
        {
            var settings = await _context.SASettings.FirstOrDefaultAsync();

            return settings.IsAiChatModuleEnabled;
        }

        public async Task<bool> GetPaymentCollectionModuleStatus()
        {
            var settings = await _context.SASettings.FirstOrDefaultAsync();

            return settings.IsPaymentCollectionModuleEnabled;
        }

        public async Task<bool> ChangeGeofenceModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsGeofenceModuleEnabled = !settings.IsGeofenceModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeIpBasedAttendanceModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsIpBasedAttendanceModuleEnabled = !settings.IsIpBasedAttendanceModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeUidLoginModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsUidLoginModuleEnabled = !settings.IsUidLoginModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeClientVisitModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsClientVisitModuleEnabled = !settings.IsClientVisitModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeDataImportExportModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsDataImportExportModuleEnabled = !settings.IsDataImportExportModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeSiteModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsSiteModuleEnabled = !settings.IsSiteModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeQrCodeAttendanceModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsQrCodeAttendanceModuleEnabled = !settings.IsQrCodeAttendanceModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeBreakModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsBreakModuleEnabled = !settings.IsBreakModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeOfflineTrackingModuleStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsOfflineTrackingModuleEnabled = !settings.IsOfflineTrackingModuleEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<string> GetWebBaseUrl()
        {
            var data = await _context.SASettings
                .Select(x => x.WebBaseUrl)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(data)) return "";

            return data;
        }

        public async Task<bool> IsImportExportModuleEnabled()
        {
            return await _context.SASettings
                .Select(x => x.IsDataImportExportModuleEnabled)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ChangeDynamicQrCodeAttendanceStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsDynamicQrCodeAttendanceEnabled = !settings.IsDynamicQrCodeAttendanceEnabled;
                settings.UpdatedAt = DateTime.Now;

                _context.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> ChangeBiometricVerificationStatus()
        {
            try
            {
                var settings = await _context.SASettings.FirstOrDefaultAsync();

                settings.IsBiometricVerificationRequired = !settings.IsBiometricVerificationRequired;
                settings.UpdatedAt = DateTime.Now;

                _context.SASettings.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
