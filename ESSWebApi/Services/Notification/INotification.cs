using ESSDataAccess.Dtos.API_Dtos.Notification;

namespace ESSWebApi.Services.Notification
{
    public interface INotification
    {
        
        Task<bool> SetAsReaded(int? Id);
        Task<List<NotificationDto>> GetNotifications();
    }
}
