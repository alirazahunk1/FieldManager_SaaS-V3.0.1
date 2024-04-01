using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class SupportController : BaseController
    {
        public SupportController(IToastNotification toast) : base(toast)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
