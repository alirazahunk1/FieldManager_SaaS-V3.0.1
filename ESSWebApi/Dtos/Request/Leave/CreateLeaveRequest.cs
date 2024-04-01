
namespace ESSWebApi.Dtos.Request.Leave
{
    public class CreateLeaveRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int LeaveType { get; set; }

        public string Comments { get; set; }
    }
}
