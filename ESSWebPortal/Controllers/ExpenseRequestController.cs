using ESSCommon.Core.Services.PushNotification;
using ESSCommon.Core.Settings;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSWebPortal.ViewModels.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class ExpenseRequestController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPushNotification _notification;
        private readonly IDbSettings _settings;
        private readonly IToastNotification _toast;

        public ExpenseRequestController(AppDbContext context,
            UserManager<AppUser> userManager,
            IPushNotification notification,
            IDbSettings settings,
            IToastNotification toastNotification)
        {
            _context = context;
            _userManager = userManager;
            _notification = notification;
            _settings = settings;
            _toast = toastNotification;
        }

        // GET: ExpenseRequest
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ExpenseRequests.Include(e => e.ExpenseType).Include(e => e.User);
            return View(await appDbContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null || _context.ExpenseRequests == null)
            {
                return NotFound();
            }

            var expenseRequestModel = await _context.ExpenseRequests
                .Include(x => x.User)
                .Include(x => x.ExpenseType)
                .FirstOrDefaultAsync(x => x.Id == id);

            ExpenseApproveVM vm = new ExpenseApproveVM
            {
                RequestId = expenseRequestModel.Id,
                ClaimedAmount = expenseRequestModel.Amount,
                CreatedAt = expenseRequestModel.CreatedAt.ToString("dd/MM/yyyy hh:mm tt"),
                ForDate = expenseRequestModel.ForDate.ToString("dd/MM/yyyy"),
                EmployeeName = expenseRequestModel.User.GetFullName(),
                ImgUrl = expenseRequestModel.ImgUrl,
                Remarks = expenseRequestModel.Remarks
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(ExpenseApproveVM vm)
        {
            var userId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                var request = await _context.ExpenseRequests.FirstOrDefaultAsync(x => x.Id == vm.RequestId);
                request.ApprovedOn = request.UpdatedAt = DateTime.Now;
                request.Approvedby = Convert.ToInt32(userId);
                request.ApprovedAmount = vm.ApprovedAmount;
                request.Status = ExpenseStatusEnum.Approved;
                request.ApproverRemarks = vm.ApproverRemarks;

                _context.Update(request);
                await _context.SaveChangesAsync();

                // await _notification.UpdateExpenseStatus(request.Approvedby.Value, request.UserId, request.ApprovedAmount.ToString(), "approved");
                await _notification.SendMessageToUser(request.UserId, $"Your expense request of amount {await _settings.GetCurrencySymbol()} {request.ApprovedAmount} is approved");
                _toast.AddSuccessToastMessage("Approved");
                return RedirectToAction(nameof(Index));
            }

            _toast.AddErrorToastMessage("Validation error");
            return View(vm);
        }

        // GET: ExpenseRequest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ExpenseRequests == null)
            {
                return NotFound();
            }

            var expenseRequestModel = await _context.ExpenseRequests
                .Include(e => e.ExpenseType)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseRequestModel == null)
            {
                return NotFound();
            }

            return View(expenseRequestModel);
        }

        // GET: ExpenseRequest/Create
        public IActionResult Create()
        {
            ViewData["ExpenseTypeId"] = new SelectList(_context.ExpenseTypes, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();
        }

        // POST: ExpenseRequest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Approvedby,ApprovedOn,ExpenseTypeId,Amount,ApprovedAmount,ExpenseStatus,Remarks,Id,CreatedAt,UpdatedAt")] ExpenseRequestModel expenseRequestModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expenseRequestModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExpenseTypeId"] = new SelectList(_context.ExpenseTypes, "Id", "Name", expenseRequestModel.ExpenseTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", expenseRequestModel.UserId);
            return View(expenseRequestModel);
        }

        // GET: ExpenseRequest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ExpenseRequests == null)
            {
                return NotFound();
            }

            var expenseRequestModel = await _context.ExpenseRequests.FindAsync(id);
            if (expenseRequestModel == null)
            {
                return NotFound();
            }
            ViewData["ExpenseTypeId"] = new SelectList(_context.ExpenseTypes, "Id", "Name", expenseRequestModel.ExpenseTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", expenseRequestModel.UserId);
            return View(expenseRequestModel);
        }

        // POST: ExpenseRequest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Approvedby,ApprovedOn,ExpenseTypeId,Amount,ApprovedAmount,ExpenseStatus,Remarks,Id,CreatedAt,UpdatedAt,TenantId")] ExpenseRequestModel expenseRequestModel)
        {
            if (id != expenseRequestModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenseRequestModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseRequestModelExists(expenseRequestModel.Id))
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
            ViewData["ExpenseTypeId"] = new SelectList(_context.ExpenseTypes, "Id", "Name", expenseRequestModel.ExpenseTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", expenseRequestModel.UserId);
            return View(expenseRequestModel);
        }
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var request = await _context.ExpenseRequests.FindAsync(id);
            var userId = _userManager.GetUserId(User);

            if (userId == null) return NotFound();

            if (request != null)
            {
                request.Status = status.Equals("approve") ? ExpenseStatusEnum.Approved : ExpenseStatusEnum.Rejected;
                request.Approvedby = Convert.ToInt32(userId);
                request.ApprovedOn = request.UpdatedAt = DateTime.Now;

                _context.Update(request);
                await _context.SaveChangesAsync();
            }
            //ToastSuccessMessage("Status updated");
            return RedirectToAction(nameof(Index));
        }
        // GET: ExpenseRequest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ExpenseRequests == null)
            {
                return NotFound();
            }

            var expenseRequestModel = await _context.ExpenseRequests
                .Include(e => e.ExpenseType)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseRequestModel == null)
            {
                return NotFound();
            }

            return View(expenseRequestModel);
        }

        // POST: ExpenseRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ExpenseRequests == null)
            {
                return Problem("Entity set 'AppDbContext.ExpenseRequests'  is null.");
            }
            var expenseRequestModel = await _context.ExpenseRequests.FindAsync(id);
            if (expenseRequestModel != null)
            {
                _context.ExpenseRequests.Remove(expenseRequestModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseRequestModelExists(int id)
        {
            return (_context.ExpenseRequests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
