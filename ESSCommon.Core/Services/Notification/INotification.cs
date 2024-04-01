using ESSCommon.Core.SharedModels.Notification;

namespace ESSCommon.Core.Services.Notification
{
    public interface INotification
    {
        Task<List<AdminNotificationVM>> GetAdminNotifications(int skip = 0, int take = 10);

        Task<bool> PostLeaveRequest(int userId);

        Task<bool> UpdateLeaveStatus(int fromUserId, int toUserId, string status);

        Task<bool> PostExpenseRequest(int userId, string amount);

        Task<bool> UpdateExpenseStatus(int fromUserId, int toUserId, string amount, string status);

        Task<bool> PostCheckInOut(int userId, AttendanceNotificationType type);

        Task<bool> PostChatMessage(int userId, string message);

        Task<bool> SendGpsWarning(int userId, GpsStatusEnum status);
    }

    public enum AttendanceNotificationType
    {
        CheckIn,
        CheckOut,
    }

    public enum GpsStatusEnum
    {
        Off,
        On
    }
}
