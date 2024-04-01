using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Models;
using ESSDataAccess.Tenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class VisitController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ITenant _tenant;
        private readonly IToastNotification _toast;

        public VisitController(AppDbContext context,
            ITenant tenant,
            IToastNotification toast)
        {
            _context = context;
            _tenant = tenant;
            _toast = toast;
        }

        public async Task<IActionResult> Index(DateTime? date, int? emp)
        {
            ViewData["employees"] = await _context.Users
              .Where(x => x.ParentId != null && x.TeamId != null && x.Status == UserStatus.Active)
              .Where(x => x.TenantId == _tenant.TenantId)
              .OrderBy(x => x.FirstName)
              .Select(x => new SelectListItem { Text = x.GetFullName(), Value = x.Id.ToString(), Selected = emp != null && emp.Value == x.Id })
              .ToListAsync();

            var visits = new List<VisitModel>();
            if (date != null && emp != null && emp.Value != 0)
            {
                ViewData["date"] = date.Value.ToString("yyyy-MM-dd");
                visits = await _context.Visits
                    .Where(x => x.CreatedAt.Date == date.Value.Date && x.CreatedBy == emp)
               .Include(x => x.Attendance).ThenInclude(x => x.Employee)
               .Include(x => x.Client)
               .ToListAsync();
            }
            else
            {
                visits = await _context.Visits
                .Include(x => x.Attendance).ThenInclude(x => x.Employee)
                .Include(x => x.Client)
                .ToListAsync();
            }

            return View(visits);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var visit = await _context.Visits.FindAsync(id.Value);

            if (visit == null) return NotFound();

            _context.Remove(visit);
            await _context.SaveChangesAsync();

            _toast.AddInfoToastMessage("Successfully deleted");
            return RedirectToAction(nameof(Index));
        }
    }
}
