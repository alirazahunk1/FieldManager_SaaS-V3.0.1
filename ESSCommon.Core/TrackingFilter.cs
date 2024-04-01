using ESSDataAccess.Models;

namespace ESSCommon.Core
{
    public class TrackingFilter
    {

        public List<TrackingModel> GetFilteredData(List<TrackingModel> trackings)
        {
            List<TrackingModel> trackingModels = new List<TrackingModel>();

            List<TrackingModel> nearbyTrackings = new List<TrackingModel>();

            bool isChecking = false;

            //check count
            if (trackings.Count() == 0)
            {
                return new List<TrackingModel>();
            }


            int stillRemoveCount = 0, inVehicleRemoveCount = 0;

            for (int i = 0; i < trackings.LongCount(); i++)
            {
                var tracking = trackings[i];

                isChecking = true;
                //Check In Out record directly add without filter
                if (tracking.Type == TrackingTypeEnum.CheckedIn || tracking.Type == TrackingTypeEnum.CheckedOut)
                {
                    trackingModels.Add(tracking);
                    continue;
                }

                var lastRec = trackingModels.Last();
                if (tracking.Latitude != null && tracking.Longitude != null
                    && tracking.Latitude != 0 && tracking.Longitude != 0)
                {
                    var distance = GetDistance(Convert.ToDouble(lastRec.Latitude), Convert.ToDouble(lastRec.Longitude),
                        Convert.ToDouble(tracking.Latitude), Convert.ToDouble(tracking.Longitude));
                    if (distance < 0.5)
                    {
                        stillRemoveCount++;
                    }
                    else if (distance < 15 && tracking.Activity == "ActivityType.IN_VEHICLE")
                    {
                        inVehicleRemoveCount++;
                    }
                    else
                    {
                        trackingModels.Add(tracking);
                    }
                }
            }

            return trackingModels.Take(24).ToList();
        }

        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }

        double ToRadians(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
