namespace ESSWebApi.Manager.ManagerDtos.Result.LeaveRequest
{
    public class LeaveRequestResult
    {

        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }
    }
}
