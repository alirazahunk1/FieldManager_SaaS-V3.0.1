using AutoMapper;
using ESSCommon.Core.SharedModels;
using ESSCommon.Core.SharedModels.Settings;
using ESSDataAccess.DbContext;
using ESSDataAccess.Models;
using ESSDataAccess.Tenant;
using Microsoft.EntityFrameworkCore;

namespace ESSCommon.Core.Settings
{
    public class DbSettingsService : IDbSettings
    {
        private readonly AppDbContext _context;
        private readonly ITenant _tenant;
        private readonly IMapper _mapper;

        public DbSettingsService(AppDbContext context,
            ITenant tenant,
            IMapper mapper)
        {
            _context = context;
            _tenant = tenant;
            _mapper = mapper;
        }


        public async Task<bool> UpdateMapSettings(string lat, string lng, int zoomLevel, int? userId = null)
        {
            try
            {
                var settings = await _context.AppSettings.FirstOrDefaultAsync();
                if (settings == null)
                {
                    throw new Exception("settings not added");
                }
                settings.CenterLatitude = lat;
                settings.CenterLongitude = lng;
                settings.MapZoomLevel = zoomLevel;
                settings.UpdatedAt = DateTime.Now;
                _context.Update(settings);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> GetPhoneNumberCode()
        {
            var phoneCode = await _context.AppSettings
                .Select(x => x.PhoneCountryCode)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(phoneCode)) return "+91";

            return phoneCode;
        }

        public async Task<MapSettingsDto> GetMapSettings()
        {
            var settings = await _context.AppSettings
                 .Select(x => new MapSettingsDto
                 {
                     Latitude = x.CenterLatitude,
                     Longitude = x.CenterLongitude,
                     MapZoomLevel = x.MapZoomLevel
                 }).FirstOrDefaultAsync();

            return settings;
        }

        public async Task AddSettings(SettingsDto settings, int userId)
        {
            var setting = _mapper.Map<SettingsDto, SettingsModel>(settings);

            setting.CreatedAt = setting.UpdatedAt = DateTime.Now;
            setting.UpdatedBy = setting.CreatedBy = userId;

            await _context.AppSettings.AddAsync(setting);
            await _context.SaveChangesAsync();
        }



        public async Task<SettingsDto> GetAll()
        {
            SettingsDto vm = new SettingsDto();

            var settings = await _context.AppSettings
                .AsNoTracking()
                .FirstOrDefaultAsync();

            vm.Country = settings.Country;
            vm.PhoneCountryCode = settings.PhoneCountryCode;
            vm.Currency = settings.Currency;
            vm.CurrencySymbol = settings.CurrencySymbol;
            vm.DistanceUnit = settings.DistanceUnit;
            vm.CenterLongitude = settings.CenterLongitude;
            vm.CenterLatitude = settings.CenterLatitude;
            vm.MapZoomLevel = settings.MapZoomLevel;


            return vm;
        }

        public async Task<bool> UpdateSettings(SettingsDto settings, int userId)
        {
            try
            {
                var setting = _mapper.Map<SettingsDto, SettingsModel>(settings);

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


        public async Task<bool> UpdateBasicSettings(string country, string phoneCountryCode, string currency, string currencySymbol, string distanceUnit, int? userId = null)
        {
            if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(phoneCountryCode) ||
                string.IsNullOrEmpty(currency) || string.IsNullOrEmpty(currencySymbol) ||
                string.IsNullOrEmpty(distanceUnit)) { return false; }

            try
            {
                var settings = await _context.AppSettings.FirstOrDefaultAsync();

                settings.Country = country;
                settings.PhoneCountryCode = phoneCountryCode;
                settings.Currency = currency;
                settings.CurrencySymbol = currencySymbol;
                settings.DistanceUnit = distanceUnit;
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


        public async Task<string> GetCurrencySymbol()
        {
            var currency = await _context.AppSettings
                 .Select(x => x.CurrencySymbol)
                 .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(currency)) return "$";

            return currency;
        }


        public async Task<string> GetApiBaseUrl()
        {
            var data = await _context.SASettings
               .Select(x => x.ApiBaseUrl)
               .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(data)) return "";

            return data;
        }

        public async Task<string> GetWebBaseUrl()
        {
            return await _context.SASettings
                .Select(x => x.WebBaseUrl)
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetOrderPrefix()
        {
            return await _context.AppSettings
                  .Select(x => x.OrderPrefix)
                  .FirstOrDefaultAsync();
        }

        public async Task<string> GetEmployeeCode()
        {
            return await _context.AppSettings
                .Select(x => x.EmployeeCodePrefix)
                .FirstOrDefaultAsync();
        }

        public async Task<ModuleSettingsDto> GetModuleSettings()
        {
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(x => x.Id == _tenant.TenantId);

            var module = await _context.Plans
                .Where(x => x.Id == tenant.PlanId)
                .Select(x => new ModuleSettingsDto
                {
                    IsAiChatModuleEnabled = x.IsAiChatModuleEnabled,
                    IsBreakModuleEnabled = x.IsBreakModuleEnabled,
                    IsBiometricVerificationModuleEnabled = x.IsBiometricVerificationModuleEnabled,
                    IsClientVisitModuleEnabled = x.IsClientVisitModuleEnabled,
                    IsDataImportExportModuleEnabled = x.IsDataImportExportModuleEnabled,
                    IsDynamicQrCodeAttendanceEnabled = x.IsDynamicQrCodeAttendanceEnabled,
                    IsGeofenceModuleEnabled = x.IsGeofenceModuleEnabled,
                    IsIpBasedAttendanceModuleEnabled = x.IsIpBasedAttendanceModuleEnabled,
                    IsOfflineTrackingModuleEnabled = x.IsOfflineTrackingModuleEnabled,
                    IsQrCodeAttendanceModuleEnabled = x.IsQrCodeAttendanceModuleEnabled,
                    IsSiteModuleEnabled = x.IsSiteModuleEnabled,
                    IsUidLoginModuleEnabled = x.IsUidLoginModuleEnabled,
                    IsProductModuleEnabled = x.IsProductModuleEnabled,
                    IsTaskModuleEnabled = x.IsTaskModuleEnabled,
                    IsNoticeModuleEnabled = x.IsNoticeModuleEnabled,
                    IsDynamicFormModuleEnabled = x.IsDynamicFormModuleEnabled,
                    IsExpenseModuleEnabled = x.IsExpenseModuleEnabled,
                    IsLeaveModuleEnabled = x.IsLeaveModuleEnabled,
                    IsDocumentModuleEnabled = x.IsDocumentModuleEnabled,
                    IsChatModuleEnabled = x.IsChatModuleEnabled,
                    IsLoanModuleEnabled = x.IsLoanModuleEnabled,
                    IsPaymentCollectionModuleEnabled = x.IsPaymentCollectionModuleEnabled
                }).FirstOrDefaultAsync();

            return module;
        }

        public async Task<bool> IsImportExportModuleEnabled()
        {
            try
            {
                var tenant = await _context.Tenants
                .FirstOrDefaultAsync(x => x.Id == _tenant.TenantId);

                var plan = await _context.Plans
                    .FirstOrDefaultAsync(x => x.Id == tenant.PlanId);

                return plan.IsDataImportExportModuleEnabled;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
