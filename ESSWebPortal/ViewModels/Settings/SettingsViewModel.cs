using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.ViewModels.Settings
{
    public class SettingsViewModel
    {
        //Basic

        [Display(Name = "Country Name (ex: US, INDIA)")]
        public string Country { get; set; }

        [Display(Name = "Country Code (ex: +1, +91)")]
        public string PhoneCountryCode { get; set; }

        [Display(Name = "Currency Name (ex: USD, INR)")]
        public string Currency { get; set; }

        [Display(Name = "Currency Symbol (ex: $, ₹)")]
        public string CurrencySymbol { get; set; }



        public string DistanceUnit { get; set; }

        [MaxLength(100)]
        [Display(Name = "Center Latitude")]
        public string? CenterLatitude { get; set; }

        [MaxLength(100)]
        [Display(Name = "Center Longitude")]
        public string? CenterLongitude { get; set; }

        [Display(Name = "Map Zoom Level (0-100)")]
        public int MapZoomLevel { get; set; }

        [Display(Name = "Employee Code Prefix")]
        public string EmployeeCodePrefix { get; set; }

        [Display(Name = "Order Prefix")]
        public string OrderPrefix { get; set; }

    }
}
