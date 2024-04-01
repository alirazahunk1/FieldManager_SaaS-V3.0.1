using ESSDataAccess.DbContext;
using ESSDataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class HolidayController : BaseController
    {
        private readonly AppDbContext _context;

        public HolidayController(AppDbContext context,
            IToastNotification toast) : base(toast)
        {
            _context = context;
        }

        // GET: Holiday
        public async Task<IActionResult> Index()
        {
            return View(await _context.Holidays.ToListAsync());
        }

        // GET: Holiday/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Holidays == null)
            {
                return NotFound();
            }

            var holidayModel = await _context.Holidays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (holidayModel == null)
            {
                return NotFound();
            }

            return View(holidayModel);
        }

        // GET: Holiday/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Date,Id,CreatedAt,UpdatedAt")] HolidayModel holidayModel)
        {
            if (ModelState.IsValid)
            {
                holidayModel.UpdatedAt = holidayModel.CreatedAt = DateTime.Now;
                _context.Add(holidayModel);
                await _context.SaveChangesAsync();
                ToastItemAddedMsg();
                return RedirectToAction(nameof(Index));
            }
            ToastErrorMessage("Unable to add");
            return View(holidayModel);
        }

        // GET: Holiday/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Holidays == null)
            {
                return NotFound();
            }

            var holidayModel = await _context.Holidays.FindAsync(id);
            if (holidayModel == null)
            {
                return NotFound();
            }
            return View(holidayModel);
        }

        // POST: Holiday/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Date,Id,CreatedAt,UpdatedAt,TenantId")] HolidayModel holidayModel)
        {
            if (id != holidayModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    holidayModel.UpdatedAt = DateTime.Now;
                    _context.Update(holidayModel);
                    await _context.SaveChangesAsync();
                    ToastItemAddedMsg("Updated");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HolidayModelExists(holidayModel.Id))
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
            ToastErrorMessage("Unable to update");
            return View(holidayModel);
        }

        // GET: Holiday/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Holidays == null)
            {
                return NotFound();
            }

            var holidayModel = await _context.Holidays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (holidayModel == null)
            {
                return NotFound();
            }

            return View(holidayModel);
        }

        // POST: Holiday/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Holidays == null)
            {
                return Problem("Entity set 'AppDbContext.Holidays'  is null.");
            }
            var holidayModel = await _context.Holidays.FindAsync(id);
            if (holidayModel != null)
            {
                _context.Holidays.Remove(holidayModel);
            }

            await _context.SaveChangesAsync();
            ToastSuccessMessage("Successfully removed");
            return RedirectToAction(nameof(Index));
        }

        private bool HolidayModelExists(int id)
        {
            return _context.Holidays.Any(e => e.Id == id);
        }
    }
}
