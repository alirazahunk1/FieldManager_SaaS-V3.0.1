using ESSCommon.Core.SharedModels.Settings;
using ESSDataAccess.DbContext;
using ESSWebApi.Dtos.Result;
using ESSWebApi.Dtos.Result.Settings;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Services.Settings
{
    public class SettingsService : ISettings
    {
        private readonly AppDbContext _context;

        public SettingsService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResult> GetAppSettingsAsync()
        {
            try
            {
                var adminSettings = await _context.SASettings
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                var settings = await _context.AppSettings
               .AsNoTracking()
               .FirstOrDefaultAsync();

                string updateType = "s";

                if (adminSettings != null && string.IsNullOrEmpty(adminSettings.MLocationUpdateIntervalType))
                {
                    if (adminSettings.MLocationUpdateIntervalType.ToLower() == "minutes")
                    {
                        updateType = "m";
                    }
                }

                SettingResult result = new SettingResult()
                {
                    CurrencySymbol = settings == null || string.IsNullOrEmpty(settings.CurrencySymbol) ? "" : settings.CurrencySymbol,
                    Currency = settings == null || string.IsNullOrEmpty(settings.Currency) ? "" : settings.Currency,
                    DistanceUnit = settings == null || string.IsNullOrEmpty(settings.DistanceUnit) ? "" : settings.DistanceUnit,
                    CountryPhoneCode = settings == null || string.IsNullOrEmpty(settings.PhoneCountryCode) ? "" : settings.PhoneCountryCode,

                    //Admin Settings
                    AppVersion = adminSettings == null || string.IsNullOrEmpty(adminSettings.AppVersion) ? "V1.0" : adminSettings.AppVersion,
                    PrivacyPolicyUrl = adminSettings == null || string.IsNullOrEmpty(adminSettings.MPrivacyPolicyLink) ? "" : adminSettings.MPrivacyPolicyLink,
                    LocationUpdateInterval = adminSettings == null || adminSettings.MLocationUpdateInterval == 0 ? 30 : adminSettings.MLocationUpdateInterval,
                    LocationUpdateIntervalType = updateType
                };

                return new BaseResult
                {
                    IsSuccess = true,
                    Data = result
                };
            }
            catch (Exception)
            {
                return new BaseResult
                {
                    Message = "Unable to get the settings"
                };
            }
        }

        public async Task<BaseResult> GetModuleSettings(string? tenantId)
        {
            if (!string.IsNullOrEmpty(tenantId) && await _context.Tenants.AnyAsync(x => x.Id == Convert.ToInt32(tenantId)))
            {
                var plan = await _context.Tenants
                    .AsNoTracking()
                    .Where(x => x.Id == Convert.ToInt32(tenantId))
                    .Select(x => x.Plan)
                    .FirstOrDefaultAsync();

                if (plan != null)
                {
                    return new BaseResult
                    {
                        IsSuccess = true,
                        Data = new ModuleSettingsDto()
                        {
                            IsProductModuleEnabled = plan.IsProductModuleEnabled,
                            IsTaskModuleEnabled = plan.IsTaskModuleEnabled,
                            IsNoticeModuleEnabled = plan.IsNoticeModuleEnabled,
                            IsDynamicFormModuleEnabled = plan.IsDynamicFormModuleEnabled,
                            IsExpenseModuleEnabled = plan.IsExpenseModuleEnabled,
                            IsLeaveModuleEnabled = plan.IsLeaveModuleEnabled,
                            IsLoanModuleEnabled = plan.IsLoanModuleEnabled,
                            IsDocumentModuleEnabled = plan.IsDocumentModuleEnabled,
                            IsChatModuleEnabled = plan.IsChatModuleEnabled,
                            IsPaymentCollectionModuleEnabled = plan.IsPaymentCollectionModuleEnabled,
                            IsAiChatModuleEnabled = plan.IsAiChatModuleEnabled,
                            IsClientVisitModuleEnabled = plan.IsClientVisitModuleEnabled,
                            IsGeofenceModuleEnabled = plan.IsGeofenceModuleEnabled,
                            IsIpBasedAttendanceModuleEnabled = plan.IsIpBasedAttendanceModuleEnabled,
                            IsOfflineTrackingModuleEnabled = plan.IsOfflineTrackingModuleEnabled,
                            IsUidLoginModuleEnabled = plan.IsUidLoginModuleEnabled,
                            IsBiometricVerificationModuleEnabled = plan.IsBiometricVerificationModuleEnabled,
                            IsBreakModuleEnabled = plan.IsBreakModuleEnabled,
                            IsDynamicQrCodeAttendanceEnabled = plan.IsDynamicQrCodeAttendanceEnabled,
                            IsQrCodeAttendanceModuleEnabled = plan.IsQrCodeAttendanceModuleEnabled,
                        }
                    };
                }
            }

            var settings = await _context.SASettings
              .AsNoTracking()
              .FirstOrDefaultAsync();

            ModuleSettingsDto result = new ModuleSettingsDto()
            {
                IsProductModuleEnabled = settings.IsProductModuleEnabled,
                IsTaskModuleEnabled = settings.IsTaskModuleEnabled,
                IsNoticeModuleEnabled = settings.IsNoticeModuleEnabled,
                IsDynamicFormModuleEnabled = settings.IsDynamicFormModuleEnabled,
                IsExpenseModuleEnabled = settings.IsExpenseModuleEnabled,
                IsLeaveModuleEnabled = settings.IsLeaveModuleEnabled,
                IsLoanModuleEnabled = settings.IsLoanModuleEnabled,
                IsDocumentModuleEnabled = settings.IsDocumentModuleEnabled,
                IsChatModuleEnabled = settings.IsChatModuleEnabled,
                IsPaymentCollectionModuleEnabled = settings.IsPaymentCollectionModuleEnabled,
                IsAiChatModuleEnabled = settings.IsAiChatModuleEnabled,
                IsClientVisitModuleEnabled = settings.IsClientVisitModuleEnabled,
                IsGeofenceModuleEnabled = settings.IsGeofenceModuleEnabled,
                IsIpBasedAttendanceModuleEnabled = settings.IsIpBasedAttendanceModuleEnabled,
                IsOfflineTrackingModuleEnabled = settings.IsOfflineTrackingModuleEnabled,
                IsUidLoginModuleEnabled = settings.IsUidLoginModuleEnabled,
                IsBiometricVerificationModuleEnabled = settings.IsBiometricVerificationRequired,
                IsBreakModuleEnabled = settings.IsBreakModuleEnabled,
                IsDynamicQrCodeAttendanceEnabled = settings.IsDynamicQrCodeAttendanceEnabled,
                IsQrCodeAttendanceModuleEnabled = settings.IsQrCodeAttendanceModuleEnabled,
            };

            return new BaseResult
            {
                IsSuccess = true,
                Data = result
            };
        }
    }
}
