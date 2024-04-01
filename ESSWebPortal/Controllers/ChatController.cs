using ESSCommon.Core;
using ESSCommon.Core.Services.PushNotification;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models.Chat;
using ESSWebPortal.Core.SuperAdmin;
using ESSWebPortal.ViewModels.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class ChatController : BaseController
    {
        private readonly IPushNotification _pushNotification;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISASettings _dbSettings;
        private readonly AppDbContext _context;

        public ChatController(IToastNotification toast,
            IPushNotification pushNotification,
            UserManager<AppUser> userManager,
            ISASettings dbSettings,
            AppDbContext context) : base(toast)
        {
            _pushNotification = pushNotification;
            _userManager = userManager;
            _dbSettings = dbSettings;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var teams = await _context.Teams
                .Include(x => x.Users)
                .Select(x => new ChatVM
                {
                    TeamId = x.Id,
                    TeamName = x.Name,
                    TeamDescription = x.Description,
                    MembersCount = x.Users.Count()
                }).AsNoTracking()
                .ToListAsync();

            return View(teams);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeamChat(int? teamId)
        {
            if (teamId == null) return NotFound();

            var userId = _userManager.GetUserId(User);

            var users = await _context.Users
             .Where(x => x.Status == UserStatus.Active)
             .Select(x => new AppUser
             {
                 Id = x.Id,
                 FirstName = x.FirstName,
                 LastName = x.LastName
             }).ToListAsync();


            var chats = await _context.Chats
                 .Where(x => x.TeamId == teamId)
                 .OrderByDescending(x => x.CreatedAt)
                 .Take(20)
                 .ToListAsync();

            List<ChatDataVM> chatItems = new List<ChatDataVM>();

            var apiBaseUrl = await _dbSettings.GetApiBaseUrl();

            foreach (var chat in chats.OrderBy(x => x.CreatedAt))
            {
                ChatDataVM item = new ChatDataVM
                {
                    Time = SharedHelper.GetRelativeDate(chat.CreatedAt),
                    Message = chat.Message
                };
                if (Convert.ToInt32(userId) != chat.UserId)
                {
                    var user = users.FirstOrDefault(x => x.Id == chat.UserId);
                    item.From = user == null ? "User not found" : user.GetFullName();
                }
                else
                {
                    item.IsYou = true;
                }

                if (chat.Type != ChatTypeEnum.Text)
                {
                    item.ImageUrl = apiBaseUrl + chat.ImageUrl;
                }

                chatItems.Add(item);
            }

            return Json(chatItems);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int teamId, string message)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return NotFound();
            try
            {


                ChatModel chat = new ChatModel();
                chat.CreatedAt = chat.UpdatedAt = DateTime.Now;
                chat.UserId = Convert.ToInt32(userId);
                chat.Message = message;
                chat.TeamId = teamId;

                await _context.AddAsync(chat);
                await _context.SaveChangesAsync();


                var user = await _context.Users
                    .Where(x => x.Id == Convert.ToInt32(userId))
                    .Select(x => x.GetFullName())
                    .FirstOrDefaultAsync();

                await _pushNotification.SendChatMessageToTeam(teamId, message, user);

                return Ok("Success");
            }
            catch (Exception)
            {
                return BadRequest("Unable to send message");
            }
        }
    }
}
