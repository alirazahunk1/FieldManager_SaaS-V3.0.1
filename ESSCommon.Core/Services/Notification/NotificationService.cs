using ESSCommon.Core.Services.PushNotification;
using ESSCommon.Core.Settings;
using ESSCommon.Core.SharedModels.Notification;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ESSCommon.Core.Services.Notification
{
    public class NotificationService : INotification
    {
        private readonly AppDbContext _context;
        private readonly IDbSettings _settings;
        private readonly IPushNotification _pushNotification;

        public NotificationService(AppDbContext context,
            IDbSettings settings,
            IPushNotification pushNotification)
        {
            _context = context;
            _settings = settings;
            _pushNotification = pushNotification;
        }


        public async Task<List<AdminNotificationVM>> GetAdminNotifications(int skip = 0, int take = 10)
        {
            var notifications = await _context.Notifications!
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Take(take)
                .Where(x => x.Type == NotiTypeEnum.Admin || x.Type == NotiTypeEnum.Attendance ||
                x.Type == NotiTypeEnum.Leave || x.Type == NotiTypeEnum.Expense ||
                x.Type == NotiTypeEnum.Conveyance || x.Type == NotiTypeEnum.GPSStatus)
                .Select(x => new AdminNotificationVM
                {
                    Id = x.Id,
                    Title = x.Title,
                    FromUserId = x.FromUserId,
                    Type = x.Type,
                    Description = x.Description,
                    CreatedAt = SharedHelper.GetRelativeDate(x.CreatedAt),
                }).AsNoTracking()
                .ToListAsync();

            return notifications;
        }

        public async Task<bool> PostChatMessage(int userId, string message)
        {
            try
            {
                var notification = new NotificationModel
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = userId,
                    FromUserId = userId,
                    Type = NotiTypeEnum.Chat,
                    Title = $"{await GetEmployeeName(userId)} has sent {message}",
                };
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();

                await _pushNotification.SendPushToAdmin(notification.Title, Enum.NotificationType.Chat);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> PostCheckInOut(int userId, AttendanceNotificationType type)
        {
            try
            {
                var notification = new NotificationModel
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = userId,
                    FromUserId = userId,
                    Type = NotiTypeEnum.Attendance,
                    Title = $"{await GetEmployeeName(userId)} is puched {(type == AttendanceNotificationType.CheckIn ? "In" : "Out")} at {DateTime.Now.ToString("hh:mm tt")}",
                };
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();

                await _pushNotification.SendPushToAdmin(notification.Title, Enum.NotificationType.Attendance);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> PostExpenseRequest(int userId, string amount)
        {
            try
            {
                var notification = new NotificationModel
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = userId,
                    FromUserId = userId,
                    Type = NotiTypeEnum.Expense,
                    Title = $"Expense request of amount {await _settings.GetCurrencySymbol()}{amount} is submitted by {await GetEmployeeName(userId)}",
                };
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();

                await _pushNotification.SendPushToAdmin(notification.Title, Enum.NotificationType.Expense);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> PostLeaveRequest(int userId)
        {
            try
            {
                var notification = new NotificationModel
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = userId,
                    FromUserId = userId,
                    Type = NotiTypeEnum.Leave,
                    Title = $"Leave request is submitted by {await GetEmployeeName(userId)}",
                };
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();

                await _pushNotification.SendPushToAdmin(notification.Title, Enum.NotificationType.Leave);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendGpsWarning(int userId, GpsStatusEnum status)
        {

            try
            {
                var notification = new NotificationModel
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = userId,
                    FromUserId = userId,
                    Type = NotiTypeEnum.GPSStatus,
                    Title = $"{await GetEmployeeName(userId)}'s GPS is turned {status}",
                };
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();

                await _pushNotification.SendPushToAdmin(notification.Title, Enum.NotificationType.Leave);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateExpenseStatus(int fromUserId, int toUserId, string amount, string status)
        {
            try
            {
                var notification = new NotificationModel
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    FromUserId = fromUserId,
                    ToUserId = toUserId,
                    Type = NotiTypeEnum.ExpenseApproval,
                    Title = $"Your expense request of amount {await _settings.GetCurrencySymbol()}{amount} is {status}",
                };
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();

                await _pushNotification.SendMessageToUser(toUserId, notification.Title);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateLeaveStatus(int fromUserId, int toUserId, string status)
        {
            try
            {
                var notification = new NotificationModel
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    FromUserId = fromUserId,
                    ToUserId = toUserId,
                    Type = NotiTypeEnum.LeaveApproval,
                    Title = $"Your leave request is {status}",
                };
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();

                await _pushNotification.SendMessageToUser(toUserId, notification.Title);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<string> GetEmployeeName(int userId)
        {
            var user = await _context.Users
                .Where(x => x.Status == UserStatus.Active)
                .Where(x => x.Id == userId)
                .Select(x => new AppUser
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                }).AsNoTracking()
                .FirstOrDefaultAsync();

            return user == null ? "" : user.GetFullName();
        }
    }
}
