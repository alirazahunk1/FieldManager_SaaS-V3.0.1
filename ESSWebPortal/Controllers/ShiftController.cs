using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSWebPortal.ViewModels.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class ShiftController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ShiftController(AppDbContext context,
            IToastNotification toast,
            UserManager<AppUser> userManager) : base(toast)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Block(int? shiftId)
        {
            if (shiftId is null) return NotFound();

            var schedule = await _context.Schedules.FindAsync(shiftId.Value);

            if (schedule is null) return NotFound();

            if (schedule.Status == ScheduleStatus.Active)
            {
                schedule.Status = ScheduleStatus.Inactive;
                ToastSuccessMessage("Shift is disabled");
            }
            else
            {
                schedule.Status = ScheduleStatus.Active;
                ToastSuccessMessage("Shift is enabled");
            }

            _context.Update(schedule);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Schedule
        public async Task<IActionResult> Index()
        {
            var schedule = await _context.Schedules
                .Select(x => new ScheduleVM
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    StartTime = x.StartTime.ToString("dd/MM/yyyy hh:mm"),
                    EndTime = x.EndTime.ToString("dd/MM/yyyy hh:mm"),
                    Status = x.Status,
                    Sunday = x.Sunday,
                    Monday = x.Monday,
                    Tuesday = x.Tuesday,
                    Wednesday = x.Wednesday,
                    Thursday = x.Thursday,
                    Friday = x.Friday,
                    Saturday = x.Saturday
                })
                .ToListAsync();

            return _context.Schedules != null ?
                        View(schedule) :
                        Problem("Entity set 'AppDbContext.Schedules'  is null.");
        }

        // GET: Schedule/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var scheduleModel = await _context.Schedules
                .Where(m => m.Id == id)
                .Select(x => new ScheduleVM
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    StartTime = x.StartTime.ToString("dd/MM/yyyy hh:mm"),
                    EndTime = x.EndTime.ToString("dd/MM/yyyy hh:mm"),
                    Status = x.Status,
                    Sunday = x.Sunday,
                    Monday = x.Monday,
                    Tuesday = x.Tuesday,
                    Wednesday = x.Wednesday,
                    Thursday = x.Thursday,
                    Friday = x.Friday,
                    Saturday = x.Saturday
                })
                .FirstOrDefaultAsync();
            if (scheduleModel == null)
            {
                return NotFound();
            }

            return Ok(scheduleModel);
        }

        // GET: Schedule/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Schedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,StartTime,EndTime,CreatedBy,Status,Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Id,CreatedAt,UpdatedAt")] ScheduleModel scheduleModel)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null) return NotFound();

                scheduleModel.CreatedAt = scheduleModel.UpdatedAt = DateTime.Now;
                scheduleModel.CreatedBy = Convert.ToInt32(userId);
                _context.Add(scheduleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scheduleModel);
        }

        // GET: Schedule/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var scheduleModel = await _context.Schedules.FindAsync(id);
            if (scheduleModel == null)
            {
                return NotFound();
            }
            return View(scheduleModel);
        }

        // POST: Schedule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,StartTime,EndTime,CreatedBy,Status,Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Id,CreatedAt,UpdatedAt,TenantId")] ScheduleModel scheduleModel)
        {
            if (id != scheduleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    scheduleModel.UpdatedAt = DateTime.Now;
                    _context.Update(scheduleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleModelExists(scheduleModel.Id))
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
            return View(scheduleModel);
        }

        // GET: Schedule/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var scheduleModel = await _context.Schedules
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduleModel == null)
            {
                return NotFound();
            }

            return View(scheduleModel);
        }

        // POST: Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Schedules == null)
            {
                return Problem("Entity set 'AppDbContext.Schedules'  is null.");
            }
            var scheduleModel = await _context.Schedules.FindAsync(id);
            if (scheduleModel != null)
            {
                _context.Schedules.Remove(scheduleModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleModelExists(int id)
        {
            return (_context.Schedules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
