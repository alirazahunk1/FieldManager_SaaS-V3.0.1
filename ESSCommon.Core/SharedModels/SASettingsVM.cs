using ESSDataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace ESSCommon.Core.SharedModels
{
    public class SASettingsVM
    {
        //Basic Settings
        public string AppName { get; set; }

        public string AppVersion { get; set; }

        [StringLength(20)]
        public string Country { get; set; }

        [StringLength(4)]
        public string PhoneCountryCode { get; set; }

        [StringLength(15)]
        public string Currency { get; set; }

        [MaxLength(4)]
        public string CurrencySymbol { get; set; }

        //Mobile App Settings
        public string MAppVersion { get; set; }

        [MaxLength(10)]
        public string? MLocationUpdateIntervalType { get; set; }

        public int? MLocationUpdateInterval { get; set; }

        [MaxLength(100)]
        public string MPrivacyPolicyLink { get; set; }

        [MaxLength(200)]
        public string ApiBaseUrl { get; set; }

        [MaxLength(200)]
        public string WebBaseUrl { get; set; }

        //Paypal settings

        [MaxLength(500)]
        public string? PaypalClientId { get; set; }


        [MaxLength(500)]
        public string? PaypalClientSecret { get; set; }

        public PaypalModeEnum? PaypalMode { get; set; }

        //Twilio
        public bool TwilioIsEnabled { get; set; }

        public string? TwilioAccountSid { get; set; }

        public string? TwilioAuthToken { get; set; }

        public string? TwilioFromNumber { get; set; }

        //Dashboard
        public string OfflineCheckTimeType { get; set; }
        public int OfflineCheckTime { get; set; }

        //Razorpay
        public string? RazorPayKeyId { get; set; }


        public string? RazorPayKeySecret { get; set; }

        public PaymentGateway PaymentGateway { get; set; }
    }
}
