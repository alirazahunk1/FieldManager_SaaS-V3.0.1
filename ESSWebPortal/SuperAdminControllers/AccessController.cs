using ESSDataAccess;
using ESSDataAccess.Identity;
using ESSWebPortal.ACL;
using ESSWebPortal.ACL.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;

namespace ESSWebPortal.SuperAdminControllers
{
    [Authorize(UserRoles.SuperAdmin)]
    public class AccessController : Controller
    {
        private readonly IMvcControllerDiscovery _mvcControllerDiscovery;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IToastNotification _notify;

        public AccessController(IMvcControllerDiscovery mvcControllerDiscovery,
            RoleManager<AppRole> roleManager,
            IToastNotification notify)
        {
            _mvcControllerDiscovery = mvcControllerDiscovery;
            _roleManager = roleManager;
            _notify = notify;
        }

        // GET: Role
        public async Task<ActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return View(roles);
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();

            return View();
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();
                return View(viewModel);
            }
            if (viewModel.SelectedControllers == null)
            {
                _notify.AddErrorToastMessage("Please select atleast one page access");
                ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();
                return View(viewModel);
            }

            var role = new AppRole { Name = viewModel.Name };
            if (viewModel.SelectedControllers != null && viewModel.SelectedControllers.Any())
            {
                foreach (var controller in viewModel.SelectedControllers)
                    foreach (var action in controller.Actions)
                        action.ControllerId = controller.Id;

                var accessJson = JsonConvert.SerializeObject(viewModel.SelectedControllers);
                role.Access = accessJson;
            }

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();

            return View(viewModel);
        }

        // GET: Role/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            //var selectedControllers = JsonConvert.DeserializeObject<IEnumerable<MvcControllerInfo>>(role.Access);
            RoleViewModel viewModel = new RoleViewModel();

            viewModel.Name = role.Name;

            if (role.Access != null)
            {
                viewModel.SelectedControllers = JsonConvert.DeserializeObject<IEnumerable<MvcControllerInfo>>(role.Access);

            }

            return View(viewModel);
        }

        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, RoleViewModel viewModel)
        {
            /*if (!ModelState.IsValid)
            {
                ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();
                return View(viewModel);
            }*/

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ModelState.AddModelError("", "Role not found");
                ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();
                return View();
            }

            role.Name = viewModel.Name;
            if (viewModel.SelectedControllers != null && viewModel.SelectedControllers.Any())
            {
                foreach (var controller in viewModel.SelectedControllers)
                    foreach (var action in controller.Actions)
                        action.ControllerId = controller.Id;

                var accessJson = JsonConvert.SerializeObject(viewModel.SelectedControllers);
                role.Access = accessJson;
            }

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            if (Constants.IsDemoMode)
            {
                _notify.AddInfoToastMessage("You cannot delete roles in demo mode");
                return RedirectToAction(nameof(Index));
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                _notify.AddErrorToastMessage("Role not found");
                return RedirectToAction(nameof(Index));
            }

            if (role.Name.ToLower() == UserRoles.Admin)
            {
                _notify.AddErrorToastMessage("YOU_CANNOT_DELETE_ADMIN_ROLE");
                return RedirectToAction(nameof(Index));
            }
            else if (role.Name.ToLower() == UserRoles.Manager)
            {
                _notify.AddErrorToastMessage("YOU_CANNOT_DELETE_Manager_ROLE");
                return RedirectToAction(nameof(Index));
            }
            else if (role.Name.ToLower() == UserRoles.Employee)
            {
                _notify.AddErrorToastMessage("YOU_CANNOT_DELETE_User_ROLE");
                return RedirectToAction(nameof(Index));
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                _notify.AddSuccessToastMessage("Role deleted");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
