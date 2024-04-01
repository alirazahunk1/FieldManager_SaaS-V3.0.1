using ESSDataAccess.DbContext;
using ESSDataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class ExpenseTypeController : Controller
    {
        private readonly AppDbContext _context;

        public ExpenseTypeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ExpenseType
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExpenseTypes.ToListAsync());
        }

        // GET: ExpenseType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ExpenseTypes == null)
            {
                return NotFound();
            }

            var expenseTypeModel = await _context.ExpenseTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseTypeModel == null)
            {
                return NotFound();
            }

            return View(expenseTypeModel);
        }

        // GET: ExpenseType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExpenseType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,IsImgRequired")] ExpenseTypeModel expenseTypeModel)
        {
            if (ModelState.IsValid)
            {
                expenseTypeModel.CreatedAt = expenseTypeModel.UpdatedAt = DateTime.Now;
                _context.Add(expenseTypeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenseTypeModel);
        }

        // GET: ExpenseType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ExpenseTypes == null)
            {
                return NotFound();
            }

            var expenseTypeModel = await _context.ExpenseTypes.FindAsync(id);
            if (expenseTypeModel == null)
            {
                return NotFound();
            }
            return View(expenseTypeModel);
        }

        // POST: ExpenseType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,IsImgRequired,CreatedAt,UpdatedAt,TenantId")] ExpenseTypeModel expenseTypeModel)
        {
            if (id != expenseTypeModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenseTypeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseTypeModelExists(expenseTypeModel.Id))
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
            return View(expenseTypeModel);
        }

        // GET: ExpenseType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ExpenseTypes == null)
            {
                return NotFound();
            }

            var expenseTypeModel = await _context.ExpenseTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseTypeModel == null)
            {
                return NotFound();
            }

            return View(expenseTypeModel);
        }

        // POST: ExpenseType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ExpenseTypes == null)
            {
                return Problem("Entity set 'AppDbContext.ExpenseTypes'  is null.");
            }
            var expenseTypeModel = await _context.ExpenseTypes.FindAsync(id);
            if (expenseTypeModel != null)
            {
                _context.ExpenseTypes.Remove(expenseTypeModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseTypeModelExists(int id)
        {
            return (_context.ExpenseTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
