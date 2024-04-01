using CZ.Api.Base;
using CZ.Api.Base.Extensions;
using ESSCommon.Core;
using ESSCommon.Core.Services.PushNotification;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models.Chat;
using ESSWebApi.Dtos.Response;
using ESSWebApi.Routes;
using ESSWebPortal.Core.SuperAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Controllers
{
    [ApiController, Authorize]
    public class ChatController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISASettings _dbSettings;
        private readonly IPushNotification _pushNotification;

        public ChatController(AppDbContext context,
            IWebHostEnvironment webHostEnvironment,
            ISASettings dbSettings,
            IPushNotification pushNotification)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _dbSettings = dbSettings;
            _pushNotification = pushNotification;
        }

        [HttpPost(APIRoutes.Chat.PostChat)]
        public async Task<IActionResult> PostChat([FromBody] string message)
        {
            if (string.IsNullOrEmpty(message)) return BadRequest("Message is required");

            var user = await _context.Users
                .Select(x => new AppUser { Id = x.Id, TeamId = x.TeamId })
                .FirstOrDefaultAsync(x => x.Id == GetUserId());

            if (user == null) return BadRequest("Employee not found");

            if (user.TeamId == null) return BadRequest("Employee not attached to a team");

            ChatModel chat = new ChatModel();
            chat.UserId = user.Id;
            chat.TeamId = user.TeamId.Value;
            chat.Message = message;
            chat.CreatedAt = chat.UpdatedAt = DateTime.Now;

            await _context.AddAsync(chat);
            await _context.SaveChangesAsync();

            await _pushNotification.SendChatMessageToTeam(user.Id, user.TeamId.Value, message);

            return Ok("Added");
        }

        [HttpPost(APIRoutes.Chat.PostImageChat)]
        public async Task<IActionResult> PostImageChat([FromForm] IFormFile? file)
        {
            if (file == null) return BadRequest("Image is required");

            if (file.Length == 0) return BadRequest("Image is required");

            try
            {
                var user = await _context.Users
              .Select(x => new AppUser { Id = x.Id, TeamId = x.TeamId })
              .FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

                if (user == null) return BadRequest("Employee not found");

                if (user.TeamId == null) return BadRequest("Employee not attached to a team");

                ChatModel chat = new ChatModel();
                chat.UserId = user.Id;
                chat.TeamId = user.TeamId.Value;
                chat.ImageUrl = await SaveFile("Images", file);
                chat.Type = ChatTypeEnum.Image;
                chat.CreatedAt = chat.UpdatedAt = DateTime.Now;

                await _context.AddAsync(chat);
                await _context.SaveChangesAsync();

                await _pushNotification.SendChatMessageToTeam(user.Id, user.TeamId.Value, "Shared an image");
                //await _notification.PostChatMessage(chat.UserId, "Shared an image");

                return Ok("Added");
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to send message");
            }
        }

        [HttpGet(APIRoutes.Chat.GetChats)]
        public async Task<IActionResult> GetChats(int skip = 0, int take = 10)
        {
            ChatResponse response = new ChatResponse();

            try
            {
                var userId = GetUserId();
                var users = await _context.Users
                    .Where(x => x.Status == UserStatus.Active)
                    .Select(x => new AppUser { Id = x.Id, FirstName = x.FirstName, LastName = x.LastName, TeamId = x.TeamId })
                    .ToListAsync();

                var user = users.Where(x => x.Id == userId).FirstOrDefault();

                if (user == null) return BadRequest("Employee not found");

                if (user.TeamId == null) return BadRequest("Employee not attached to a team");

                var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == user.TeamId);

                if (team == null) return BadRequest("Invalid team");

                var chats = await _context.Chats
                    .Where(x => x.TeamId == user.TeamId)
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                response.TeamName = team.Name;
                response.TeamId = team.Id;
                response.IsChatEnabled = team.IsChatEnabled;

                response.ChatItems = new List<ChatItem>();
                var apiBaseUrl = await _dbSettings.GetApiBaseUrl();

                if (chats.Count() > 0)
                {
                    foreach (var chat in chats)
                    {
                        string? from = null;

                        if (userId != chat.UserId)
                        {
                            var frm = users.FirstOrDefault(x => x.Id == chat.UserId);
                            from = frm != null ? frm.GetFullName() : "";
                        }

                        response.ChatItems.Add(new ChatItem
                        {
                            Id = chat.Id,
                            From = from,
                            Message = chat.Message,
                            ChatType = chat.Type.ToString(),
                            FileUrl = apiBaseUrl + chat.ImageUrl,
                            CreatedAt = SharedHelper.GetRelativeDate(chat.CreatedAt),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to get chats" + ex.StackTrace);
            }

            return Ok(response);
        }

        [NonAction]
        private async Task<string> SaveFile(string fileType, IFormFile file)
        {
            var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/Chats", fileType);

            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var fullPath = Path.Combine(pathToSave, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("uploads/Chats", fileType, fileName);
        }
    }
}
