using ESSDataAccess.DbContext;
using ESSWebPortal.ViewModels.Device;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class DeviceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IToastNotification _toast;

        public DeviceController(AppDbContext context,
            IToastNotification toast)
        {
            _context = context;
            _toast = toast;
        }

        public async Task<IActionResult> Index()
        {
            var devices = await _context.UserDevices
                .Include(x => x.User)
                .Select(x => new DeviceVM
                {
                    Id = x.Id,
                    Board = x.Board,
                    Brand = x.Brand,
                    Model = x.Model,
                    DeviceType = x.DeviceType,
                    LastUpdatedOn = x.UpdatedAt,
                    SdkVersion = x.SdkVersion,
                    FullName = x.User.GetFullName(),
                    UserId = x.User.Id,
                }).ToListAsync();

            return View(devices);
        }

        public async Task<IActionResult> RevokeDevice(int? id)
        {
            if (id == null) return NotFound();

            var device = await _context.UserDevices
                .FirstOrDefaultAsync(x => x.Id == id.Value);

            if (device != null)
            {
                _context.Remove(device);
                await _context.SaveChangesAsync();
                _toast.AddSuccessToastMessage("Device removed");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
