namespace ESSWebApi.Dtos.Result.Device
{
    public class StatusUpdateResult
    {
        public string AttendanceType { get; set; }

        public string UserStatus { get; set; } = "active";

        public string Status { get; set; }

        public string CheckInAt { get; set; }

        public string CheckOutAt { get; set; }


        public string ShiftStartTime { get; set; }

        public string ShiftEndTime { get; set; }

        public bool? IsLate { get; set; }

        public bool? IsOnBreak { get; set; }

        public string? BreakStartedAt { get; set; }

        public bool IsOnLeave { get; set; } = false;

        public decimal? TravelledDistance { get; set; }

        public int? TrackedHours { get; set; }


        //Site

        public bool IsSiteEmployee { get; set; }

        public string SiteName { get; set; }

        public string SiteAttendanceType { get; set; }

    }
}
