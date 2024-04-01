using ESSWebApi.Dtos.Result;

namespace ESSWebApi.Services.Settings
{
    public interface ISettings
    {
        Task<BaseResult> GetAppSettingsAsync();

        Task<BaseResult> GetModuleSettings(string? tenantId);
    }
}
