

namespace ESSWebApi.Dtos.Result.Leave
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string LeaveType { get; set; }

        public string Comments { get; set; }

        public string Status { get; set; }

        public string Image { get; set; }

        public string CreatedOn { get; set; }

        public string ApprovedOn { get; set; }

        public string ApprovedBy { get; set; }
    }
}
