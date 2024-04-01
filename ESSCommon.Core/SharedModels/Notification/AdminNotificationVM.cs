using ESSDataAccess.Models;

namespace ESSCommon.Core.SharedModels.Notification
{
    public class AdminNotificationVM
    {
        public int Id { get; set; }

        public int? FromUserId { get; set; }

        public string? FromUser { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public NotiTypeEnum Type { get; set; }

        public string CreatedAt { get; set; }
    }
}
