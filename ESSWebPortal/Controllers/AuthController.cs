using ESSCommon.Core.Subscription;
using ESSDataAccess;
using ESSDataAccess.DbContext;
using ESSDataAccess.Identity;
using ESSDataAccess.Tenant;
using ESSDataAccess.Tenant.Models;
using EssWebPortal;
using ESSWebPortal.Core.ViewModel.Auth;
using ESSWebPortal.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITenant _tenant;
        private readonly ISubscription _subscription;
        private readonly IToastNotification _toast;

        public AuthController(
            AppDbContext context,
            RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITenant tenant,
            ISubscription subscription,
            IToastNotification toast)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _tenant = tenant;
            _subscription = subscription;
            _toast = toast;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (ModelState.IsValid)
            {
                register.PhoneNumber = register.PhoneNumber.Trim();
                register.UserName = register.UserName.Trim().ToLower();
                register.FirstName = register.FirstName.Trim();
                register.LastName = register.LastName.Trim();

                if (await _context.Users.AnyAsync(x => x.UserName == register.UserName))
                {
                    _toast.AddErrorToastMessage("User name already taken");
                    return View(register);
                }

                if (await _context.Users.AnyAsync(x => x.PhoneNumber == register.PhoneNumber))
                {
                    _toast.AddErrorToastMessage("Phone number already exists");
                    return View(register);
                }

                //Tenant Creation
                TenantModel tenant = new TenantModel
                {
                    Name = register.UserName,
                    Description = register.FirstName + " " + register.LastName + " " + register.UserName,
                    NormalizedName = register.UserName.ToUpper(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                await _context.AddAsync(tenant);
                await _context.SaveChangesAsync();

                AppUser user = new AppUser
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    UserName = register.UserName.ToLower(),
                    NormalizedUserName = register.UserName.ToUpper(),
                    Email = register.UserName.ToLower(),
                    NormalizedEmail = register.UserName.ToUpper(),
                    Gender = register.Gender,
                    CreatedAt = DateTime.Now,
                    PhoneNumber = register.PhoneNumber,
                    Status = ESSDataAccess.Enum.UserStatus.InActive,
                    TenantId = tenant.Id,
                };

                var result = await _userManager.CreateAsync(user, register.ConfirmPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (user.TenantId.HasValue)
                    {
                        _tenant.SetTenant(user.TenantId.Value);

                        Response.Cookies.Append("Tenant", user.TenantId.Value.ToString());
                    }

                    _toast.AddSuccessToastMessage("Account Successfully Created!");
                    return RedirectToAction("Index", "Payment");
                }
                _toast.AddErrorToastMessage("Error");
                return View(register);
            }

            return View(register);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (!await _context.Users.AnyAsync())
            {
                try
                {
                    Response.Cookies.Delete("Tenant");
                    _tenant.ClearTenant();

                    DataSeeder seeder = new DataSeeder(_context, _userManager, _tenant, _roleManager);
                    //Demo
                    //await seeder.SeedDataToDb();

                    //Live
                    await seeder.SeedLiveDataToDb();

                    _toast.AddAlertToastMessage("Demo data seeded to db");
                }
                catch (Exception ex)
                {
                    _toast.AddAlertToastMessage("Unable to seed demo data");
                }

            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login([Bind("UserName", "Password", "RememberMe")] LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.FindByNameAsync(vm.UserName!);

            if (user is null)
            {
                _toast.AddErrorToastMessage("Invalid Username");
                return View(vm);
            }

            var role = await _userManager.GetRolesAsync(user);

            var isSuperAdmin = await _userManager.IsInRoleAsync(user, UserRoles.SuperAdmin);


            if (!(role.FirstOrDefault() == UserRoles.SuperAdmin || role.FirstOrDefault() == UserRoles.Admin))
            {
                _toast.AddErrorToastMessage("You don't have access");
                return View(vm);
            }


            var isValidPassword = await _userManager.CheckPasswordAsync(user, vm.Password!);

            if (!isValidPassword)
            {
                _toast.AddErrorToastMessage("Wrong password");
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, false);

            if (!result.Succeeded)
            {
                _toast.AddErrorToastMessage("Something went wrong");
                return View(vm);
            }

            if (isSuperAdmin)
            {
                _tenant.ClearTenant();
                _toast.AddSuccessToastMessage("Welcome back!");
                return RedirectToAction("Index", "SuperAdmin");
            }

            if (user.TenantId.HasValue)
            {
                _tenant.SetTenant(user.TenantId.Value);

                Response.Cookies.Append("Tenant", user.TenantId.Value.ToString());
            }


            //Subscription check
            var isSubscriptionActive = await _subscription.IsSubscriptionActive(user.TenantId.Value);

            var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == user.TenantId);

            if (tenant is null)
            {
                _toast.AddErrorToastMessage("Invalid tenant data");
                return View(vm);
            }


            if (!isSubscriptionActive)
            {
                _toast.AddErrorToastMessage("Please renew your subscription");
                return RedirectToAction("Index", "Payment");
            }

            if (!await _context.AppSettings.IgnoreQueryFilters().AnyAsync(x => x.TenantId == user.TenantId))
            {
                _toast.AddInfoToastMessage("Please add your settings first");
                return RedirectToAction("AddInitialSettings", "Settings");
            }

            _toast.AddSuccessToastMessage("Welcome back!");

            return RedirectToAction("Index", "Dashboard");
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("Tenant");
            await _signInManager.SignOutAsync();

            _toast.AddInfoToastMessage("Logged out successfully");

            return RedirectToAction(nameof(Login));
        }


    }
}
