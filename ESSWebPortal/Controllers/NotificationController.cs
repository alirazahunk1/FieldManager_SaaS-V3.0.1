using ESSCommon.Core.Enum;
using ESSCommon.Core.Services.PushNotification;
using ESSDataAccess.DbContext;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSDataAccess.Tenant;
using ESSWebPortal.ViewModels.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class NotificationController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IPushNotification _notification;
        private readonly ITenant _tenant;
        private readonly IToastNotification _toast;

        public NotificationController(
            AppDbContext context,
            IPushNotification notification,
            ITenant tenant,
            IToastNotification toast) : base(toast)
        {
            _context = context;
            _notification = notification;
            _tenant = tenant;
            _toast = toast;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var notifications = await _context.Notifications
                .Select(x => new NotificationVM
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    FromUserId = x.FromUserId,
                    Description = x.Description,
                    Title = x.Title,
                    Type = x.Type,
                }).AsNoTracking()
                .ToListAsync();
            return View(notifications);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["users"] = await _context.Users
                .Where(x => x.Status == ESSDataAccess.Enum.UserStatus.Active)
                .Where(x => x.TenantId == _tenant.TenantId)
                .Where(x => x.ScheduleId != null && x.TeamId != null)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GetFullName() })
                .ToListAsync();

            ViewData["teams"] = await _context.Teams
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                }).ToListAsync();


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNotificationVM vm)
        {
            try
            {
                string tokens = string.Empty;
                List<string> userDevices = new List<string>();
                switch (vm.Type)
                {
                    case NotificationTypeEnumVM.All:

                        var devices = await _context.UserDevices
                       .Where(x => x.Token != null)
                       .Select(x => x.Token)
                       .AsNoTracking()
                       .ToListAsync();
                        foreach (var dev in devices)
                        {
                            if (string.IsNullOrEmpty(dev)) continue;

                            userDevices.Add(dev);
                        }
                        break;
                    case NotificationTypeEnumVM.Teams:

                        var teams = await _context.Teams
                            .Include(x => x.Users).ThenInclude(x => x.UserDevice)
                            .Select(x => new TeamModel
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Users = x.Users
                            }).ToListAsync();

                        foreach (var team in vm.TeamId)
                        {
                            var usersInTeams = teams.FirstOrDefault(x => x.Id == Convert.ToInt32(team)).Users;

                            if (usersInTeams == null || usersInTeams.Count() == 0) continue;

                            foreach (var user in usersInTeams)
                            {
                                if (user.UserDevice == null || string.IsNullOrEmpty(user.UserDevice.Token)) continue;

                                userDevices.Add(user.UserDevice.Token);
                            }
                        }

                        break;
                    case NotificationTypeEnumVM.Employees:
                        var users = await _context.Users
                            .Where(x => x.Status == ESSDataAccess.Enum.UserStatus.Active)
                            .Where(x => x.TenantId == _tenant.TenantId)
                            .Include(x => x.UserDevice)
                            .Select(x => new AppUser
                            {
                                Id = x.Id,
                                UserDevice = x.UserDevice
                            }).AsNoTracking()
                            .ToListAsync();

                        foreach (var usr in vm.UserId)
                        {
                            var user = users.FirstOrDefault(x => x.Id == Convert.ToInt32(usr));

                            if (user == null || user.UserDevice == null || string.IsNullOrEmpty(user.UserDevice.Token)) continue;

                            userDevices.Add(user.UserDevice.Token);
                        }

                        break;
                    default:
                        break;
                }


                tokens = string.Join(",", userDevices);
                if (!string.IsNullOrEmpty(tokens))
                {

                    await _notification.SendMessage(NotificationType.Announcement, tokens, vm.Message);

                }
                _toast.AddSuccessToastMessage("Message Sent");

                return RedirectToAction(nameof(Create));
            }
            catch (Exception)
            {

            }
            _toast.AddErrorToastMessage("Unable to send message");
            return View(vm);
        }

    }
}
