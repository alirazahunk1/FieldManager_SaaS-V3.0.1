using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using ESSDataAccess.Models.Site;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class AttendanceModel : BaseModel
    {

        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }

        public AppUser? Employee { get; set; }

        public DateTime CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        [MaxLength(500)]
        public string? LateReason { get; set; }

        [MaxLength(500)]
        public string? EarlyCheckOutReason { get; set; }

        public AttendanceStatus Status { get; set; }

        [Display(Name = "Approved By")]
        public int? ApprovedById { get; set; }

        [Display(Name = "Approved On")]
        public DateTime? ApprovedOn { get; set; }

        [ForeignKey(nameof(Shift))]
        public int? ShiftId { get; set; }

        public ScheduleModel? Shift { get; set; }

        [ForeignKey(nameof(Site))]
        public int? SiteId { get; set; }

        public SiteModel? Site { get; set; }

        public List<TrackingModel>? Trackings { get; set; }

        public List<VisitModel>? Visits { get; set; }

        public List<CheckInOutModel>? CheckInOuts { get; set; }

        public List<BreakModel>? Breaks { get; set; }
    }

}
