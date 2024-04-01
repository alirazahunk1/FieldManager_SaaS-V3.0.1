using ESSCommon.Core.Enum;
using ESSCommon.Core.Services.PushNotification;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class LeaveRequestController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPushNotification _pushNotification;

        public LeaveRequestController(AppDbContext context,
            UserManager<AppUser> userManager,
            IPushNotification pushNotification,
            IToastNotification toast) : base(toast)
        {
            _context = context;
            _userManager = userManager;
            _pushNotification = pushNotification;
        }

        // GET: LeaveRequest
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.LeaveRequests.Include(l => l.User).Include(l => l.LeaveType);
            return View(await appDbContext.ToListAsync());
        }

        // GET: LeaveRequest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LeaveRequests == null)
            {
                return NotFound();
            }

            var leaveRequestModel = await _context.LeaveRequests
                .Include(l => l.User)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveRequestModel == null)
            {
                return NotFound();
            }

            return View(leaveRequestModel);
        }

        public async Task<IActionResult> DetailsAjax(int? id)
        {
            if (id == null || _context.LeaveRequests == null)
            {
                return NotFound();
            }

            var leaveRequestModel = await _context.LeaveRequests
                .Include(l => l.User)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveRequestModel == null)
            {
                return NotFound();
            }

            var vm = new
            {
                Id = leaveRequestModel.Id,
                FromDate = leaveRequestModel.FromDate.ToString("dd/MM/yyyy"),
                ToDate = leaveRequestModel.ToDate.ToString("dd/MM/yyyy"),
                LeaveType = leaveRequestModel.LeaveType.Name,
                ImgUrl = leaveRequestModel.Document,
                Comments = leaveRequestModel.Comments,
                User = leaveRequestModel.User.GetFullName(),
                ApprovedBy = leaveRequestModel.ApprovedBy,
                ApprovedOn = leaveRequestModel.ApprovedOn.HasValue ? leaveRequestModel.ApprovedOn.Value.ToString("dd/MM/yyyy") : string.Empty,
                Status = leaveRequestModel.Status,
                createdAt = leaveRequestModel.CreatedAt.ToString("dd/MM/yyyy")
            };

            return Json(vm);
        }

        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var request = await _context.LeaveRequests.FindAsync(id);
            var userId = _userManager.GetUserId(User);

            if (userId == null) return NotFound();

            if (request != null)
            {
                request.Status = status.Equals("approve") ? LeaveRequestStatus.Approved : LeaveRequestStatus.Rejected;
                request.ApprovedBy = Convert.ToInt32(userId);
                request.ApprovedOn = request.UpdatedAt = DateTime.Now;

                _context.Update(request);
                await _context.SaveChangesAsync();
                await _pushNotification.SendMessageToUser(request.UserId, $"Your leave request has been {status}ed", NotificationType.Leave);

            }
            ToastSuccessMessage("Status updated");
            return RedirectToAction(nameof(Index));
        }

        // GET: LeaveRequest/Create
        public IActionResult Create()
        {
            ViewData["RequestedBy"] = new SelectList(_context.Users, "Id", "FirstName");
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            return View();
        }

        // POST: LeaveRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FromDate,ToDate,LeaveTypeId,Document,Comments,RequestedBy,ApprovedBy,ApprovedOn,Status,Id,CreatedAt,UpdatedAt")] LeaveRequestModel leaveRequestModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaveRequestModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RequestedBy"] = new SelectList(_context.Users, "Id", "FirstName", leaveRequestModel.UserId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveRequestModel.LeaveTypeId);
            return View(leaveRequestModel);
        }

        // GET: LeaveRequest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LeaveRequests == null)
            {
                return NotFound();
            }

            var leaveRequestModel = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequestModel == null)
            {
                return NotFound();
            }
            ViewData["RequestedBy"] = new SelectList(_context.Users, "Id", "FirstName", leaveRequestModel.UserId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveRequestModel.LeaveTypeId);
            return View(leaveRequestModel);
        }

        // POST: LeaveRequest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FromDate,ToDate,LeaveTypeId,Document,Comments,RequestedBy,ApprovedBy,ApprovedOn,Status,Id,CreatedAt,UpdatedAt,TenantId")] LeaveRequestModel leaveRequestModel)
        {
            if (id != leaveRequestModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    leaveRequestModel.UpdatedAt = DateTime.Now;
                    _context.Update(leaveRequestModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveRequestModelExists(leaveRequestModel.Id))
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
            ViewData["RequestedBy"] = new SelectList(_context.Users, "Id", "FirstName", leaveRequestModel.UserId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveRequestModel.LeaveTypeId);
            return View(leaveRequestModel);
        }

        // GET: LeaveRequest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LeaveRequests == null)
            {
                return NotFound();
            }

            var leaveRequestModel = await _context.LeaveRequests
                .Include(l => l.User)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveRequestModel == null)
            {
                return NotFound();
            }

            return View(leaveRequestModel);
        }

        // POST: LeaveRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LeaveRequests == null)
            {
                return Problem("Entity set 'AppDbContext.LeaveRequests'  is null.");
            }
            var leaveRequestModel = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequestModel != null)
            {
                _context.LeaveRequests.Remove(leaveRequestModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveRequestModelExists(int id)
        {
            return (_context.LeaveRequests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
