using ESSCommon.Core.SharedModels;
using ESSCommon.Core.SharedModels.Settings;

namespace ESSCommon.Core.Settings
{
    public interface IDbSettings
    {
        Task<string> GetPhoneNumberCode();

        Task<MapSettingsDto> GetMapSettings();

        Task<bool> UpdateMapSettings(string lat, string lng, int zoomLevel, int? userId = null);

        Task AddSettings(SettingsDto settings, int userId);

        Task<bool> UpdateBasicSettings(string country, string phoneCountryCode, string currency, string currencySymbol, string distanceUnit, int? userId = null);

        Task<string> GetCurrencySymbol();

        Task<string> GetApiBaseUrl();

        Task<SettingsDto> GetAll();

        Task<string> GetWebBaseUrl();

        Task<string> GetOrderPrefix();

        Task<string> GetEmployeeCode();

        Task<ModuleSettingsDto> GetModuleSettings();

        Task<bool> IsImportExportModuleEnabled();
    }
}
