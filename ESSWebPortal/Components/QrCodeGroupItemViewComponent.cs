using ESSDataAccess.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESSWebPortal.Components
{
    public class QrCodeGroupItemViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public QrCodeGroupItemViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int groupId)
        {
            var items = await _context.QrCodes
                .Where(x => x.QrCodeGroupId == groupId)
                .ToListAsync();

            return View(items);
        }
    }
}
