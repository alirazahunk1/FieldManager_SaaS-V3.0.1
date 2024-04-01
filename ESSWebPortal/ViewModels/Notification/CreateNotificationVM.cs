using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.ViewModels.Notification
{
    public class CreateNotificationVM
    {
        public bool IsSchedule { get; set; } = false;

        public DateTime? ScheduleTime { get; set; }

        public string Message { get; set; }

        public NotificationTypeEnumVM Type { get; set; }

        [Display(Name = "Users")]
        public List<string>? UserId { get; set; }

        [Display(Name = "Teams")]
        public List<string>? TeamId { get; set; }

    }

    public enum NotificationTypeEnumVM
    {
        All,
        Teams,
        Employees,
    }
}
