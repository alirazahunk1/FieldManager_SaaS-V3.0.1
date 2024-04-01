using CZ.Api.Base;
using ESSWebApi.Routes;
using ESSWebApi.Services.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESSWebApi.Controllers
{
    [Authorize, ApiController]
    public class SettingsController : BaseController
    {
        private readonly ISettings _settings;

        public SettingsController(ISettings settings)
        {
            _settings = settings;
        }

        [AllowAnonymous]
        [HttpGet(APIRoutes.Settings.GetAppSettings)]
        public async Task<IActionResult> GetAppSettings()
        {
            var result = await _settings.GetAppSettingsAsync();

            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [AllowAnonymous]
        [HttpGet(APIRoutes.Settings.GetModuleSettings)]
        public async Task<IActionResult> GetModuleSettings()
        {
            var tenantId = Request.Headers["TenantId"].ToString();

            var result = await _settings.GetModuleSettings(tenantId);

            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(result.Data);
        }
    }
}
