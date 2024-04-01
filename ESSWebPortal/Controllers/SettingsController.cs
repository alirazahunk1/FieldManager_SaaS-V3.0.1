using ESSCommon.Core.Settings;
using ESSDataAccess.DbContext;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSWebPortal.ViewModels.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        private readonly IDbSettings _dbSettings;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public SettingsController(IToastNotification toast,
            IDbSettings dbSettings,
            UserManager<AppUser> userManager,
            AppDbContext context) : base(toast)

        {
            _dbSettings = dbSettings;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetNewSettings());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SaveAppSettings(string country, string phoneCountryCode, string currency, string currencySymbol, string distanceUnit)
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                ToastErrorMessage("You cannot save settings in demo mode");
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(phoneCountryCode) ||
                string.IsNullOrEmpty(currency) || string.IsNullOrEmpty(currencySymbol) ||
                string.IsNullOrEmpty(distanceUnit))
            {
                ToastErrorMessage("Values cannot be empty");
                return RedirectToAction(nameof(Index));
            }

            var result = await _dbSettings.UpdateBasicSettings(country, phoneCountryCode, currency, currencySymbol, distanceUnit);

            if (!result)
            {
                ToastErrorMessage("Unable to save the settings"); return RedirectToAction(nameof(Index));
            }

            ToastSuccessMessage("App Settings Saved");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SaveMapSettings(string? centerLatitude, string? centerLongitude, int? mapZoomLevel)
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                ToastErrorMessage("You cannot save settings in demo mode");
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(centerLatitude) || string.IsNullOrEmpty(centerLongitude) || mapZoomLevel == null)
            {
                ToastErrorMessage("Values cannot be empty");
                return RedirectToAction(nameof(Index));
            }

            if (mapZoomLevel.Value < 0 || mapZoomLevel.Value > 100)
            {
                ToastErrorMessage("Zoom level should be 0-100");
                return RedirectToAction(nameof(Index));
            }

            var result = await _dbSettings.UpdateMapSettings(centerLatitude, centerLongitude, mapZoomLevel.Value);

            if (!result)
            {
                ToastErrorMessage("Unable to save the settings");
            }
            else
            {
                ToastSuccessMessage("Map Settings Saved");
            }


            return RedirectToAction(nameof(Index));
        }


        public async Task<SettingsViewModel> GetNewSettings()
        {
            try
            {
                SettingsViewModel vm = new SettingsViewModel();

                //Basic Settings
                var appSettings = await _dbSettings.GetAll();
                vm.Country = appSettings.Country;
                vm.PhoneCountryCode = appSettings.PhoneCountryCode;
                vm.Currency = appSettings.Currency;
                vm.CurrencySymbol = appSettings.CurrencySymbol;
                vm.DistanceUnit = appSettings.DistanceUnit;
                vm.CenterLatitude = appSettings.CenterLatitude;
                vm.CenterLongitude = appSettings.CenterLongitude;
                vm.MapZoomLevel = appSettings.MapZoomLevel;

                return vm;
            }
            catch (Exception)
            {
                return new SettingsViewModel();
            }
        }

        public async Task<IActionResult> AddInitialSettings()
        {
            if (await _context.AppSettings.AnyAsync())
            {
                return RedirectToAction(nameof(Index), "Dashboard");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddInitialSettings(SettingsViewModel vm)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(User);
                SettingsModel model = new SettingsModel
                {
                    Country = vm.Country,
                    PhoneCountryCode = vm.PhoneCountryCode,
                    Currency = vm.Currency,
                    CurrencySymbol = vm.CurrencySymbol,
                    DistanceUnit = vm.DistanceUnit,
                    TenantId = user.TenantId.Value,
                    CenterLatitude = vm.CenterLatitude,
                    CenterLongitude = vm.CenterLongitude,
                    MapZoomLevel = vm.MapZoomLevel,
                    CreatedBy = user.Id,
                    EmployeeCodePrefix = vm.EmployeeCodePrefix,
                    OrderPrefix = vm.OrderPrefix,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                await _context.AppSettings.AddAsync(model);
                await _context.SaveChangesAsync();

                ToastSuccessMessage("Settings added");
                ToastSuccessMessage("Welcome back");
                return RedirectToAction("Index", "Dashboard");

            }
            else
            {
                return View(vm);
            }
        }
    }
}
