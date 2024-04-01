using DocumentFormat.OpenXml.Presentation;
using ESSDataAccess.Models;
using Org.BouncyCastle.Asn1.Pkcs;

namespace ESSCommon.Core
{
    public class SharedHelper
    {
        const double PIx = 3.141592653589793;
        const double RADIO = 6378.16;
        public static List<DateTime> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list
        }

        public static bool IsWeekOff(ScheduleModel schedule, DateTime date)
        {
            if (schedule == null) return false;

            var week = date.DayOfWeek;
            switch (week)
            {
                case DayOfWeek.Sunday:
                    return !schedule.Sunday;
                case DayOfWeek.Monday:
                    return !schedule.Monday;
                case DayOfWeek.Tuesday:
                    return !schedule.Tuesday;
                case DayOfWeek.Wednesday:
                    return !schedule.Wednesday;
                case DayOfWeek.Thursday:
                    return !schedule.Thursday;
                case DayOfWeek.Friday:
                    return !schedule.Friday;
                case DayOfWeek.Saturday:
                    return !schedule.Saturday;
                default:
                    break;
            }
            return false;
        }

        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }
        public static double Radians(double x)
        {
            return x * PIx / 180;
        }

        public static double DistanceBetweenPlaces(
          double lon1,
          double lat1,
          double lon2,
          double lat2)
        {
            double dlon = Radians(lon2 - lon1);
            double dlat = Radians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return ((angle * RADIO) * 0.62137) * 1.6;//distance in miles
        }

        public static string GetRelativeDate(DateTime date)
        {
            Dictionary<long, string> thresholds = new Dictionary<long, string>();
            int minute = 60;
            int hour = 60 * minute;
            int day = 24 * hour;
            thresholds.Add(60, "{0} seconds ago");
            thresholds.Add(minute * 2, "a minute ago");
            thresholds.Add(45 * minute, "{0} minutes ago");
            thresholds.Add(120 * minute, "an hour ago");
            thresholds.Add(day, "{0} hours ago");
            thresholds.Add(day * 2, "yesterday");
            thresholds.Add(day * 30, "{0} days ago");
            thresholds.Add(day * 365, "{0} months ago");
            thresholds.Add(long.MaxValue, "{0} years ago");
            long since = (DateTime.Now.Ticks - date.Ticks) / 10000000;
            foreach (long threshold in thresholds.Keys)
            {
                if (since < threshold)
                {
                    TimeSpan t = new TimeSpan((DateTime.Now.Ticks - date.Ticks));
                    return string.Format(thresholds[threshold], (t.Days > 365 ? t.Days / 365 : (t.Days > 0 ? t.Days : (t.Hours > 0 ? t.Hours : (t.Minutes > 0 ? t.Minutes : (t.Seconds > 0 ? t.Seconds : 0))))).ToString());
                }
            }
            return "";
        }
    }
}
