using ESSCommon.Core.Services.PushNotification;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSDataAccess.Models.Chat;
using ESSDataAccess.Tenant;
using ESSWebPortal.ViewModels.Employee;
using ESSWebPortal.ViewModels.Team;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IToastNotification _toast;
        private readonly ITenant _tenant;
        private readonly IPushNotification _pushNotification;

        private class ChatM
        {
            public bool IsActive { get; set; } = true;

            public List<ChatItem> ChatItems { get; set; }
        }

        private partial class ChatItem
        {
            public int Id { get; set; }

            public string From { get; set; }

            public string Message { get; set; }

            public string CreatedAt { get; set; }
        }

        public TeamController(AppDbContext context,
            UserManager<AppUser> userManager,
            IToastNotification toast,
            ITenant tenant,
            IPushNotification pushNotification)
        {
            _context = context;
            _userManager = userManager;
            _toast = toast;
            _tenant = tenant;
            _pushNotification = pushNotification;
        }



        [HttpGet]
        public async Task<IActionResult> Block(int? teamId)
        {
            if (teamId is null) return NotFound();

            var team = await _context.Teams
                .Where(x => x.Id == teamId.Value).FirstOrDefaultAsync();

            if (team is null) return NotFound();

            if (team.Status == TeamStatus.Active)
            {
                team.Status = TeamStatus.InActive;
                _toast.AddSuccessToastMessage("Team is disabled");
            }
            else
            {
                team.Status = TeamStatus.Active;
                _toast.AddSuccessToastMessage("Team is enabled");
            }

            _context.Update(team);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Team
        public async Task<IActionResult> Index()
        {
            var teams = await _context.Teams
                         .Include(x => x.Users)
                         .Select(x => new TeamVM
                         {
                             Id = x.Id,
                             Description = x.Description,
                             Name = x.Name,
                             Status = x.Status,
                             UserCount = x.Users.Count()
                         })
                         .ToListAsync();

            return View(teams);
        }


        public async Task<IActionResult> Chat(int? id)
        {
            var team = await _context.Teams.FindAsync(id);

            ViewData["team"] = team.Name;
            ViewData["id"] = id;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetChatDataAjax(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return NotFound();

            var users = await _context.Users
                .Where(x => x.Status == UserStatus.Active)
                .Where(x => x.TenantId == _tenant.TenantId)
                .Select(x => new AppUser
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                }).ToListAsync();

            var chats = await _context.Chats
                .Where(x => x.TeamId == id)
                .OrderByDescending(x => x.CreatedAt)
                .Take(20)
                .ToListAsync();
            if (chats == null) return NotFound();

            ChatM chatModel = new ChatM();

            chatModel.ChatItems = new List<ChatItem>();
            foreach (var chat in chats)
            {
                string? from = null;
                if (Convert.ToInt32(userId) != chat.UserId)
                {
                    var user = users.FirstOrDefault(x => x.Id == chat.UserId);
                    from = user == null ? "User not found" : user.GetFullName();
                }

                chatModel.ChatItems.Add(new ChatItem
                {
                    CreatedAt = chat.CreatedAt.ToString(),
                    From = from,
                    Id = chat.Id,
                    Message = chat.Message
                });
            }

            return Json(chatModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetUsersAjax(int id)
        {
            var users = await _context.Users
                .Where(x => x.Status == UserStatus.Active)
                .Where(x => x.TenantId == _tenant.TenantId)
                .Where(x => x.TeamId == id)
                .Select(x => new EmployeeVM
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    Address = x.Address,
                    Email = x.Email,
                    Gender = x.Gender,
                    Status = x.Status
                })
                .ToListAsync();

            return Json(users);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int id, string message)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return NotFound();

            ChatModel chat = new ChatModel();
            chat.CreatedAt = chat.UpdatedAt = DateTime.Now;
            chat.UserId = Convert.ToInt32(userId);
            chat.Message = message;
            chat.TeamId = id;

            await _context.AddAsync(chat);
            await _context.SaveChangesAsync();


            await _pushNotification.SendChatMessageToTeam(id, message);

            return Ok("Success");
        }

        // GET: Team/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var teamModel = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamModel == null)
            {
                return NotFound();
            }

            return View(teamModel);
        }

        // GET: Team/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Team/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Id")] TeamModel teamModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                teamModel.Status = TeamStatus.Active;
                teamModel.CreatedAt = teamModel.UpdatedAt = DateTime.Now;
                _context.Add(teamModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teamModel);
        }

        // GET: Team/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var teamModel = await _context.Teams.FindAsync(id);
            if (teamModel == null)
            {
                return NotFound();
            }
            return View(teamModel);
        }

        // POST: Team/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Status,Id,CreatedAt,UpdatedAt,TenantId")] TeamModel teamModel)
        {
            if (id != teamModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    teamModel.UpdatedAt = DateTime.Now;
                    _context.Update(teamModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamModelExists(teamModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teamModel);
        }

        // GET: Team/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var teamModel = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamModel == null)
            {
                return NotFound();
            }

            return View(teamModel);
        }

        // POST: Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teams == null)
            {
                return Problem("Entity set 'AppDbContext.Teams'  is null.");
            }
            var teamModel = await _context.Teams.FindAsync(id);
            if (teamModel != null)
            {
                _context.Teams.Remove(teamModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamModelExists(int id)
        {
            return (_context.Teams?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
