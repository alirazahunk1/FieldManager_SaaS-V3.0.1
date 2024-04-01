using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class NotificationModel : BaseModel
    {
        [Required(ErrorMessage = "Notification title is required")]
        [StringLength(60, ErrorMessage = "Title can't be longer than 60 characters")]
        public string Title { get; set; }

        [StringLength(400, ErrorMessage = "Description can't be longer than 400 characters")]
        public string? Description { get; set; }

        [StringLength(400)]
        public string? Image { get; set; }

        public bool IsViewed { get; set; } = false;

        public int? FromUserId { get; set; }

        [ForeignKey(nameof(ToUser))]
        public int? ToUserId { get; set; }

        public int? TeamId { get; set; }

        public AppUser? ToUser { get; set; }

        public NotiTypeEnum Type { get; set; }

    }

    public enum NotiTypeEnum
    {
        Common,
        Announcement,
        Admin,
        Manager,
        Hr,
        Supervisor,
        Expense,
        Leave,
        Conveyance,
        Attendance,
        AttendanceOverride,
        Chat,
        Order,
        ExpenseApproval,
        LeaveApproval,
        Tracking,
        GPSStatus

    }
}
