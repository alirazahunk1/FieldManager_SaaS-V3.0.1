using ESSDataAccess.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESSWebPortal.Components
{
    public class GeofenceGroupItemViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public GeofenceGroupItemViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int groupId)
        {
            var items = await _context.Geofences
                .Where(x => x.GeofenceGroupId == groupId)
                .ToListAsync();

            return View(items);
        }
    }
}
