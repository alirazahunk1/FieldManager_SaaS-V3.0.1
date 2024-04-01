using CorePush.Google;
using ESSCommon.Core.Enum;
using ESSCommon.Core.Services.Push;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ESSCommon.Core.Services.PushNotification
{
    public class PushNotificationService : IPushNotification
    {

        private readonly IPush _push;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly FcmSettings _fcmSettings;

        public PushNotificationService(IPush push,
            AppDbContext context,
            UserManager<AppUser> userManager,
            FcmSettings fcmSettings)
        {
            _push = push;
            _context = context;
            _userManager = userManager;
            _fcmSettings = fcmSettings;
        }

        public async Task SendMessageToUser(int userId, string message, NotificationType type = NotificationType.Announcement)
        {
            var userDevice = await _context
                .UserDevices
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userDevice != null && !string.IsNullOrEmpty(userDevice.Token))
            {
                await _push.SendMessage(type, userDevice.Token, message);
            }
            NotificationModel notification = new NotificationModel();
            notification.Description = message;
            notification.ToUserId = userId;
            notification.Title = type.ToString();
            notification.UpdatedAt = notification.CreatedAt = DateTime.Now;
            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();

        }

        public async Task SendChatMessageToTeam(int userId, int teamId, string message)
        {
            try
            {
                var users = await _context.Users
                    .Where(x => x.Status == UserStatus.Active)
                    .Where(x => x.TeamId == teamId)
                    .Include(x => x.UserDevice)
                    .Select(x => new AppUser
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        UserDevice = x.UserDevice,
                    })
                    .ToListAsync();
                var userDevices = users
                    .Where(x => x.Id != userId)
                    .Select(x => x.UserDevice);


                if (users == null || userDevices == null || users.Count == 0) return;

                var fromUser = users.FirstOrDefault(x => x.Id == userId);

                if (fromUser == null) return;

                string token = string.Empty;
                token = string.Join(",", userDevices
                    .Where(x => x != null && !string.IsNullOrEmpty(x.Token))
                    .Select(x => x.Token)
                    .ToList());

                if (string.IsNullOrEmpty(token)) return;

                await _push.SendMessage(NotificationType.Chat, token, $"{fromUser.GetFullName()} sent {message}");
            }
            catch (Exception)
            {
            }
        }
        public async Task SendChatMessageToTeam(int teamId, string message, string? title = null)
        {
            try
            {
                var userDevices = await _context.Users
                    .Where(x => x.Status == UserStatus.Active)
                    .Where(x => x.TeamId == teamId)
                    .Include(x => x.UserDevice)
                    .Select(x => x.UserDevice)
                    .ToListAsync();

                if (userDevices == null) return;

                string token = string.Empty;
                token = string.Join(",", userDevices
                    .Where(x => x != null && !string.IsNullOrEmpty(x.Token))
                    .Select(x => x.Token)
                    .ToList());

                if (string.IsNullOrEmpty(token)) return;

                await _push.SendMessage(NotificationType.Chat, token, message, title);
            }
            catch (Exception)
            {
            }
        }

        public async Task<bool> SendMessage(NotificationType type, string tokens, string message)
        {
            try
            {
                HttpClient client = new HttpClient();
                var fcm = new FcmSender(_fcmSettings, client);

                var payload = new
                {
                    to = GetTopic(type),
                    notification = new
                    {
                        title = type.ToString(),
                        body = message
                    }
                };

                var result = await fcm.SendAsync(payload);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        private string GetTopic(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Announcement:
                    return "/topics/announcement";
                case NotificationType.Chat:
                    return "/topics/chat";
                case NotificationType.Attendance:
                    return "/topics/attendance";
                case NotificationType.Leave:
                    return "/topics/leave";
                case NotificationType.Expense:
                    return "/topics/expense";
                case NotificationType.Conveyance:
                    return "/topics/conveyance";
            }

            return string.Empty;
        }

        public async Task SendPushToAdmin(string message, NotificationType notificationType)
        {
            var tokens = await _context.Users
                .Where(x => x.Status == UserStatus.Active)
                .Include(x => x.PushToken)
                .Where(x => x.TeamId == null && x.ScheduleId == null)
                .Where(x => x.PushToken != null)
                .Select(x => x.PushToken.Token)
                .ToListAsync();

            if (tokens is not null && tokens.Count() > 0)
            {
                await _push.SendMessage(notificationType, string.Join(",", tokens), message);
            }
        }
    }
}
