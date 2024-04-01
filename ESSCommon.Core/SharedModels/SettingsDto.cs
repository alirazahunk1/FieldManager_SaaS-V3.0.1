using System.ComponentModel.DataAnnotations;

namespace ESSCommon.Core.SharedModels
{
    public class SettingsDto
    {

        //AppSettings
        public string AppName { get; set; }

        public string AppVersion { get; set; }

        public string Country { get; set; }

        public string PhoneCountryCode { get; set; }

        public string Currency { get; set; }

        public string CurrencySymbol { get; set; }

        public string DistanceUnit { get; set; }

        [MaxLength(100)]
        public string? CenterLatitude { get; set; }

        [MaxLength(100)]
        public string? CenterLongitude { get; set; }

        [MinLength(1), MaxLength(100)]
        public int MapZoomLevel { get; set; }
    }
}
