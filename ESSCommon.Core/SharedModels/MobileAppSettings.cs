using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSCommon.Core.SharedModels
{
    public class MobileAppSettings
    {
        public double MobileAppVersion { get; set; }

        public string? LocationUpdateIntervalType { get; set; }

        public int? LocationUpdateInterval { get; set; }

        public string? PrivacyPolicyLink { get; set; }

        public string DistanceUnit { get; set; }
    }
}
