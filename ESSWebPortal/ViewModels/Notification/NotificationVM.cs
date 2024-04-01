using ESSDataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.ViewModels.Notification
{
    public class NotificationVM
    {
        public int Id { get; set; }

        public int? FromUserId { get; set; }

        public string? FromUser { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public NotiTypeEnum Type { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedAt { get; set; }
    }
}
