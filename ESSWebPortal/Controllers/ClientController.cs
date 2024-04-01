using ESSDataAccess.DbContext;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSWebPortal.ViewModels.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IToastNotification _toast;

        public ClientController(AppDbContext context,
            UserManager<AppUser> userManager,
            IToastNotification toast)
        {
            _context = context;
            _userManager = userManager;
            _toast = toast;
        }

        public async Task<IActionResult> Index()
        {
            var client = await _context.Clients
                .Select(x => new ClientVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    PhoneNumber = x.PhoneNumber,
                    ContactPerson = x.ContactPerson,
                    Radius = x.Radius,
                    Email = x.Email,
                    City = x.City,
                    Remarks = x.Remarks,
                    Status = x.Status,
                })
                .ToListAsync();

            return View(client);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var clientModel = await _context.Clients
                .Where(m => m.Id == id)
                .Select(m => new ClientVM
                {
                    Id = m.Id,
                    Name = m.Name,
                    PhoneNumber = m.PhoneNumber,
                    Email = m.Email,
                    City = m.City,
                    Address = m.Address,
                    ContactPerson = m.ContactPerson,
                    Status = m.Status,
                    RequestedOn = m.CreatedAt.ToString("dd/MM/yyyy"),
                    Remarks = m.Remarks,
                }).FirstOrDefaultAsync();

            if (clientModel == null)
            {
                return NotFound();
            }

            return Ok(clientModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address,Latitude,Longitude,PhoneNumber,ContactPerson,Radius,Email,City,Remarks,Status,Id,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy")] ClientModel clientModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);


                clientModel.CreatedAt = clientModel.UpdatedAt = DateTime.Now;
                clientModel.CreatedBy = clientModel.UpdatedBy = user.Id;
                _context.Add(clientModel);
                await _context.SaveChangesAsync();
                _toast.AddSuccessToastMessage("Client added");
                return RedirectToAction(nameof(Index));
            }
            return View(clientModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var clientModel = await _context.Clients.FindAsync(id);
            if (clientModel == null)
            {
                return NotFound();
            }
            return View(clientModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Address,Latitude,Longitude,PhoneNumber,ContactPerson,Radius,Email,City,Remarks,Status,Id,CreatedAt,UpdatedAt,TenantId")] ClientModel clientModel)
        {
            if (id != clientModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    clientModel.UpdatedAt = DateTime.Now;
                    _context.Update(clientModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientModelExists(clientModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _toast.AddSuccessToastMessage("Client details updated");
                return RedirectToAction(nameof(Index));
            }
            return View(clientModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var clientModel = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientModel == null)
            {
                return NotFound();
            }

            return View(clientModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'AppDbContext.Clients' is null.");
            }
            var clientModel = await _context.Clients.FindAsync(id);
            if (clientModel != null)
            {
                _context.Clients.Remove(clientModel);
            }

            await _context.SaveChangesAsync();
            _toast.AddSuccessToastMessage("Client deleted");
            return RedirectToAction(nameof(Index));
        }

        private bool ClientModelExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
