using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class LeaveRequestModel : BaseModel
    {


        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        [ForeignKey(nameof(LeaveType))]

        public int LeaveTypeId { get; set; }
        [Display(Name = "Leave Type")]
        public LeaveTypeModel LeaveType { get; set; }
        public string? Document { get; set; }
        public string? Comments { get; set; }
        [ForeignKey(nameof(User))]
        [Display(Name = "Requested By")]
        public int UserId { get; set; }
        public AppUser User { get; set; }
        [Display(Name = "Approved By")]
        public int? ApprovedBy { get; set; }
        [Display(Name = "Approved On")]

        [MaxLength(200)]
        public string? ApproverRemarks { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public LeaveRequestStatus Status { get; set; }

    }


}
