namespace ESSWebApi.Dtos.Result.Attendance
{
    public class AttendanceDto
    {
        public int Id { get; set; }

        public string CheckInTime { get; set; }

        public string CheckOutTime { get; set; }

        public string Status { get; set; }

        public string ApprovedBy { get; set; }

        public string ApprovedOn { get; set; }

        public string Date { get; set; }
    }
}
