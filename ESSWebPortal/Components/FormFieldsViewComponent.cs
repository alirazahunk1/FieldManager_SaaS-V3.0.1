using ESSDataAccess.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESSWebPortal.Components
{
    public class FormFieldsViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public FormFieldsViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int formId)
        {
            var model = await _context.FormFields
                .Where(x => x.FormId == formId)
                .ToListAsync();

            return View(model);
        }
    }
}
