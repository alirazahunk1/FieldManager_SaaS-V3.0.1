using ESSDataAccess.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESSWebPortal.Components
{
    public class IpGroupItemViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public IpGroupItemViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int groupId)
        {
            var items = await _context.IpAddresses
                .Where(x => x.IpGroupId == groupId)
                .ToListAsync();

            return View(items);
        }
    }
}
