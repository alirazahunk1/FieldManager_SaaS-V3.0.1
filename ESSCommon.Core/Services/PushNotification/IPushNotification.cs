using ESSCommon.Core.Enum;

namespace ESSCommon.Core.Services.PushNotification
{
    public interface IPushNotification
    {
        Task<bool> SendMessage(NotificationType type, string tokens, string message);

        Task SendMessageToUser(int userId, string message, NotificationType type = NotificationType.Announcement);

        Task SendChatMessageToTeam(int userId, int teamId, string message);

        Task SendChatMessageToTeam(int teamId, string message, string? title = null);

        Task SendPushToAdmin(string message, NotificationType notificationType);
    }
}

