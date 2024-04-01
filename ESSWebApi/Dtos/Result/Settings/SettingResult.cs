namespace ESSWebApi.Dtos.Result.Settings
{
    public class SettingResult
    {
        public string AppVersion { get; set; }

        public string LocationUpdateIntervalType { get; set; }

        public int LocationUpdateInterval { get; set; }

        public string PrivacyPolicyUrl { get; set; }

        public string Currency { get; set; } = string.Empty;

        public string CurrencySymbol { get; set; } = string.Empty;

        public string DistanceUnit { get; set; } = string.Empty;

        public string CountryPhoneCode { get; set; } = string.Empty;

    }
}
