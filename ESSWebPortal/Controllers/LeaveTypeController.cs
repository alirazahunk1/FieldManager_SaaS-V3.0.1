using ESSDataAccess.DbContext;
using ESSDataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class LeaveTypeController : BaseController
    {
        private readonly AppDbContext _context;
        public LeaveTypeController(AppDbContext context,
            IToastNotification toast) : base(toast)
        {
            _context = context;
        }

        // GET: LeaveType
        public async Task<IActionResult> Index()
        {
            return _context.LeaveTypes != null ?
                        View(await _context.LeaveTypes.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.LeaveTypes'  is null.");
        }

        // GET: LeaveType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LeaveTypes == null)
            {
                return NotFound();
            }

            var leaveTypeModel = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveTypeModel == null)
            {
                return NotFound();
            }

            return View(leaveTypeModel);
        }

        // GET: LeaveType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveType/Creates
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,IsImgRequired")] LeaveTypeModel leaveTypeModel)
        {
            if (ModelState.IsValid)
            {
                leaveTypeModel.CreatedAt = leaveTypeModel.UpdatedAt = DateTime.Now;
                _context.Add(leaveTypeModel);
                await _context.SaveChangesAsync();

                ToastItemAddedMsg();

                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeModel);
        }

        // GET: LeaveType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LeaveTypes == null)
            {
                return NotFound();
            }

            var leaveTypeModel = await _context.LeaveTypes.FindAsync(id);
            if (leaveTypeModel == null)
            {
                return NotFound();
            }
            return View(leaveTypeModel);
        }

        // POST: LeaveType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,CreatedAt,UpdatedAt,IsImgRequired,TenantId")] LeaveTypeModel leaveTypeModel)
        {
            if (id != leaveTypeModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    leaveTypeModel.UpdatedAt = DateTime.Now;
                    _context.Update(leaveTypeModel);
                    await _context.SaveChangesAsync();
                    ToastItemEditMsg();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveTypeModelExists(leaveTypeModel.Id))
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
            return View(leaveTypeModel);
        }

        // GET: LeaveType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LeaveTypes == null)
            {
                return NotFound();
            }

            var leaveTypeModel = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveTypeModel == null)
            {
                return NotFound();
            }

            return View(leaveTypeModel);
        }

        // POST: LeaveType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LeaveTypes == null)
            {
                return Problem("Entity set 'AppDbContext.LeaveTypes'  is null.");
            }
            var leaveTypeModel = await _context.LeaveTypes.FindAsync(id);
            if (leaveTypeModel != null)
            {
                _context.LeaveTypes.Remove(leaveTypeModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveTypeModelExists(int id)
        {
            return (_context.LeaveTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
