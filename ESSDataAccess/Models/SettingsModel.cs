using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models
{
    public class SettingsModel : BaseModel
    {
        [StringLength(20)]
        public string Country { get; set; }

        [StringLength(4)]
        public string PhoneCountryCode { get; set; }

        [StringLength(15)]
        public string Currency { get; set; }

        [MaxLength(4)]
        public string CurrencySymbol { get; set; }

        [MaxLength(3)]
        public string DistanceUnit { get; set; }

        [MaxLength(100)]
        public string? CenterLatitude { get; set; }

        [MaxLength(100)]
        public string? CenterLongitude { get; set; }

        [MinLength(1), MaxLength(100)]
        public int MapZoomLevel { get; set; } = 0;

        [MaxLength(1000)]
        public string? MapKey { get; set; }

        [MaxLength(2000)]
        public string? LogoURL { get; set; }

        public bool VerifyClientNumber { get; set; }

        [MaxLength(10)]
        public string EmployeeCodePrefix { get; set; }

        //Order
        public string OrderPrefix { get; set; }
    }
}
