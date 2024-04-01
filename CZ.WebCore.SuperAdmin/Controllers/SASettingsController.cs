using ESSCommon.Core.SharedModels;
using ESSDataAccess;
using ESSDataAccess.DbContext;
using ESSDataAccess.Models;
using ESSWebPortal.Core.SuperAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace CZ.WebCore.SuperAdmin.Controllers
{
    [Authorize(UserRoles.SuperAdmin)]
    public class SASettingsController : Controller
    {
        private readonly IToastNotification _toast;
        private readonly ISASettings _SASettings;
        private readonly AppDbContext _context;

        public SASettingsController(IToastNotification toast,
            ISASettings SASettings,
            AppDbContext context)

        {
            _toast = toast;
            _SASettings = SASettings;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetNewSettings());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SaveSMSSettings(string twilioAccountSid, string twilioAuthToken, string twilioFromNumber)
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                _toast.AddErrorToastMessage("You cannot save settings in demo mode");
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(twilioAccountSid) || string.IsNullOrEmpty(twilioAuthToken) || string.IsNullOrEmpty(twilioFromNumber))
            {
                _toast.AddErrorToastMessage("Values cannot be empty");
                return RedirectToAction(nameof(Index));
            }

            var result = await _SASettings.UpdateTwilioSettings(twilioAccountSid, twilioAuthToken, twilioFromNumber);

            if (!result)
            {
                _toast.AddErrorToastMessage("Unable to save the settings"); return RedirectToAction(nameof(Index));
            }

            _toast.AddSuccessToastMessage("SMS Settings Saved");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SaveDashboardSettings(string offlineCheckTimeType, int offlineCheckTime)
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                _toast.AddErrorToastMessage("You cannot save settings in demo mode");
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(offlineCheckTimeType))
            {
                _toast.AddErrorToastMessage("Values cannot be empty");
                return RedirectToAction(nameof(Index));
            }

            var result = await _SASettings.UpdateDashboardSettings(offlineCheckTimeType, offlineCheckTime.ToString());

            if (!result)
            {
                _toast.AddErrorToastMessage("Unable to save the settings"); return RedirectToAction(nameof(Index));
            }

            _toast.AddSuccessToastMessage("Dashboard Settings Saved");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTwilioStatus(bool status)
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                _toast.AddErrorToastMessage("You cannot save settings in demo mode");
                return RedirectToAction(nameof(Index));
            }

            var result = await _SASettings.UpdateTwilioStatus(status);

            if (!result)
            {
                _toast.AddErrorToastMessage("Unable to save the settings"); return RedirectToAction(nameof(Index));
            }

            return Ok("Updated");
        }


        [HttpPost]
        public async Task<IActionResult> ChangeDefaultPaymentGateway(PaymentGateway gateway)
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                _toast.AddErrorToastMessage("You cannot save settings in demo mode");
                return RedirectToAction(nameof(Index));
            }

            var result = await _SASettings.ChangeDefaultPaymentGateway(gateway);

            if (!result)
            {
                _toast.AddErrorToastMessage("Unable to save the settings"); return RedirectToAction(nameof(Index));
            }

            return Ok("Updated");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SaveAppSettings(string appName, string appVersion, string country, string phoneCountryCode, string currency, string currencySymbol)
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                _toast.AddErrorToastMessage("You cannot save settings in demo mode");
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(appName) || string.IsNullOrEmpty(appVersion) ||
                string.IsNullOrEmpty(country) || string.IsNullOrEmpty(phoneCountryCode) ||
                string.IsNullOrEmpty(currency) || string.IsNullOrEmpty(currencySymbol))
            {
                _toast.AddErrorToastMessage("Values cannot be empty");
                return RedirectToAction(nameof(Index));
            }

            var result = await _SASettings.UpdateBasicSettings(appName, appVersion.ToString(), country, phoneCountryCode, currency, currencySymbol);

            if (!result)
            {
                _toast.AddErrorToastMessage("Unable to save the settings"); return RedirectToAction(nameof(Index));
            }

            _toast.AddSuccessToastMessage("App Settings Saved");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SaveMobileAppSettings(string mAppVersion, string mLocationUpdateIntervalType, int mLocationUpdateInterval, string mPrivacyPolicyLink, string ApiBaseUrl, string webBaseUrl)
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                _toast.AddErrorToastMessage("You cannot save settings in demo mode");
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(mAppVersion) || string.IsNullOrEmpty(mPrivacyPolicyLink) || string.IsNullOrEmpty(ApiBaseUrl) || string.IsNullOrEmpty(webBaseUrl))
            {
                _toast.AddErrorToastMessage("Values cannot be empty");
                return RedirectToAction(nameof(Index));
            }

            var result = await _SASettings
                .UpdateMobileAppSettings(mAppVersion, mLocationUpdateIntervalType, mLocationUpdateInterval.ToString(), mPrivacyPolicyLink, ApiBaseUrl, webBaseUrl);

            if (!result)
            {
                _toast.AddErrorToastMessage("Unable to save the settings"); return RedirectToAction(nameof(Index));
            }
            _toast.AddSuccessToastMessage("Mobile App Settings Saved");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SavePaypalSettings(string paypalClientId, string paypalClientSecret, PaypalModeEnum paypalMode)
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                _toast.AddErrorToastMessage("You cannot save settings in demo mode");
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(paypalClientId) || string.IsNullOrEmpty(paypalClientId))
            {
                _toast.AddErrorToastMessage("Values cannot be empty");
                return RedirectToAction(nameof(Index));
            }

            var result = await _SASettings.UpdatePaypalSettings(paypalClientId, paypalClientSecret, paypalMode);

            if (!result)
            {
                _toast.AddErrorToastMessage("Unable to save the settings"); return RedirectToAction(nameof(Index));
            }
            _toast.AddSuccessToastMessage("Paypal Settings Saved");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> SaveRazorPaySettings(string razorPayKeyId, string razorPayKeySecret)
        {
            if (ESSDataAccess.Constants.IsDemoMode)
            {
                _toast.AddErrorToastMessage("You cannot save settings in demo mode");
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(razorPayKeyId) || string.IsNullOrEmpty(razorPayKeySecret))
            {
                _toast.AddErrorToastMessage("Values cannot be empty");
                return RedirectToAction(nameof(Index));
            }


            var result = await _SASettings.UpdateRazorpaySettings(razorPayKeyId, razorPayKeySecret);

            if (!result)
            {
                _toast.AddErrorToastMessage("Unable to save the settings"); return RedirectToAction(nameof(Index));
            }
            _toast.AddSuccessToastMessage("Razorpay Settings Saved");
            return RedirectToAction(nameof(Index));
        }

        public async Task<SASettingsVM> GetNewSettings()
        {
            try
            {
                SASettingsVM vm = new SASettingsVM();

                //Basic Settings
                var appSettings = await _SASettings.GetAll();
                vm.AppName = appSettings.AppName;
                vm.AppVersion = appSettings.AppVersion;
                vm.Country = appSettings.Country;
                vm.PhoneCountryCode = appSettings.PhoneCountryCode;
                vm.Currency = appSettings.Currency;
                vm.CurrencySymbol = appSettings.CurrencySymbol;

                //Mobile App Settings
                vm.MAppVersion = appSettings.MAppVersion;
                vm.MLocationUpdateIntervalType = appSettings.MLocationUpdateIntervalType;
                vm.MLocationUpdateInterval = appSettings.MLocationUpdateInterval;
                vm.MPrivacyPolicyLink = appSettings.MPrivacyPolicyLink;
                vm.ApiBaseUrl = appSettings.ApiBaseUrl;

                //Paypal Settings
                vm.PaypalClientId = appSettings.PaypalClientId;
                vm.PaypalClientSecret = appSettings.PaypalClientSecret;
                vm.PaypalMode = appSettings.PaypalMode;


                //Dashboard
                vm.OfflineCheckTime = appSettings.OfflineCheckTime;
                vm.OfflineCheckTimeType = appSettings.OfflineCheckTimeType;

                //SMS
                vm.TwilioFromNumber = appSettings.TwilioFromNumber;
                vm.TwilioAccountSid = appSettings.TwilioAccountSid;
                vm.TwilioAuthToken = appSettings.TwilioAuthToken;
                vm.TwilioIsEnabled = appSettings.TwilioIsEnabled;

                vm.RazorPayKeyId = appSettings.RazorPayKeyId;
                vm.RazorPayKeySecret = appSettings.RazorPayKeySecret;
                vm.PaymentGateway = appSettings.PaymentGateway;

                return vm;
            }
            catch (Exception)
            {
                return new SASettingsVM();
            }
        }
    }
}
