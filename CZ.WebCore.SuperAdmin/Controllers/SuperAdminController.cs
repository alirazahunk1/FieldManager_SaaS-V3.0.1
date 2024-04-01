using CZ.WebCore.SuperAdmin.ViewModels;
using CZ.WebCore.SuperAdmin.ViewModels.Dashboard;
using ESSDataAccess;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSDataAccess.Tenant.Models;
using ESSWebPortal.Core.SuperAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace CZ.WebCore.SuperAdmin.Controllers
{
    [Authorize(UserRoles.SuperAdmin)]
    public class SuperAdminController : Controller
    {
        private readonly ISuperAdmin _superAdmin;
        private readonly AppDbContext _context;
        private readonly ISASettings _sASettings;
        private readonly IToastNotification _toast;
        private readonly UserManager<AppUser> _userManager;

        public SuperAdminController(ISuperAdmin superAdmin,
            AppDbContext context,
            ISASettings sASettings,
            IToastNotification toast,
            UserManager<AppUser> userManager)
        {
            _superAdmin = superAdmin;
            _context = context;
            _sASettings = sASettings;
            _toast = toast;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ChangePlan(int? userId)
        {
            if (userId is null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            var vm = new ChangePlanVM
            {
                UserId = userId.Value,
                TenantId = user.TenantId.Value,
                Name = user.FirstName + " " + user.LastName,
                Email = user.Email,
                UserName = user.UserName,
            };

            var currencySymbol = await _sASettings.GetCurrencySymbol();

            ViewData["Plans"] = await _context.Plans
            .Where(x => x.Status == PlanStatus.Active)
            .Select(x => new SelectListItem
            {
                Text = x.Name + " (" + currencySymbol + x.Price + ") " + x.Type.ToString(),
                Value = x.Id.ToString(),
            }).ToListAsync();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePlan(ChangePlanVM vm)
        {
            if (vm.Duration == 0)
            {
                _toast.AddErrorToastMessage("Please select duration");
                return View(vm);
            }

            if (vm.PlanId is null)
            {
                _toast.AddErrorToastMessage("Please select plan");
                return View(vm);
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == vm.UserId);

            var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == user.TenantId);

            await CreateOrder(user.TenantId.Value, vm.PlanId.Value, vm.UserId.Value, user.UserName, vm.Duration, tenant.TotalEmployeesCount, true);

            _toast.AddSuccessToastMessage("Plan changed successfully");
            return RedirectToAction(nameof(UsersList));
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> IndexAjax()
        {
            //Recent orders
            var orders = await _context.Orders
                .Include(x => x.Plan)
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .Select(x => new SARecentOrderVM
                {
                    OrderId = x.OrderId,
                    PurchaseDate = x.CreatedAt.ToString("dd/MM/yyyy"),
                    Plan = x.Plan.Name,
                    Amount = x.PerEmployeeAmount,
                    Status = x.Status,
                })
                .ToListAsync();

            return Json(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetCountAjax()
        {
            return Json(new
            {
                TotalTenants = await _context.Tenants.CountAsync(),
                Plans = await _context.Plans.CountAsync(),
                TotalSales = await _context.Orders.Where(x => x.Status == SAOrderStatus.Success).SumAsync(x => x.PerEmployeeAmount),
                Subscriptions = await _context.Tenants.CountAsync(x => x.SubscriptionStatus == ESSDataAccess.Tenant.Models.SubscriptionStatusEnum.Active)
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetSalesDataAjax()
        {
            DateTime date = DateTime.Now.AddMonths(-6);

            List<SalesChartItem> items = new List<SalesChartItem>();
            var orders = await _context.Orders
                .Where(x => x.CreatedAt > date)
                .Select(x => new OrderModel
                {
                    PerEmployeeAmount = x.PerEmployeeAmount,
                    CreatedAt = x.CreatedAt,
                }).AsNoTracking()
                .ToListAsync();

            for (int i = 0; i < 7; i++)
            {
                SalesChartItem item = new SalesChartItem
                {
                    Date = date.ToString("MMMM"),
                    Amount = orders.Where(x => x.CreatedAt.Month == date.Month).Sum(x => x.PerEmployeeAmount).ToString(),
                };
                items.Add(item);
                date = date.AddMonths(1);
            }
            return Json(items);
        }


        [HttpGet]
        public async Task<IActionResult> Activate(int? userId)
        {
            var currencySymbol = await _sASettings.GetCurrencySymbol();
            if (userId is null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            var vm = new ActivateVM
            {
                UserId = userId.Value,
                TenantId = user.TenantId.Value,
                Name = user.FirstName + " " + user.LastName,
                Email = user.Email,
                UserName = user.UserName,
            };

            ViewData["Plans"] = await _context.Plans
                .Where(x => x.Status == PlanStatus.Active)
                .Select(x => new SelectListItem
                {
                    Text = x.Name + " (" + currencySymbol + x.Price + ") " + x.Type.ToString(),
                    Value = x.Id.ToString(),
                }).ToListAsync();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Activate(ActivateVM vm)
        {
            if (vm.Duration == 0)
            {
                _toast.AddErrorToastMessage("Please select duration");
                return View(vm);
            }

            if (vm.PlanId == 0)
            {
                _toast.AddErrorToastMessage("Please select plan");
                return View(vm);
            }

            if (vm.EmployeesCount < 1)
            {
                _toast.AddErrorToastMessage("Please select employees count");
                return View(vm);
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == vm.UserId);

            await CreateOrder(vm.TenantId, vm.PlanId, vm.UserId, user.UserName, vm.Duration, vm.EmployeesCount);

            _toast.AddSuccessToastMessage("User activated successfully");
            return RedirectToAction(nameof(UsersList));
        }

        public async Task<IActionResult> UsersList()
        {
            var users = await _superAdmin.GetAdminUsers();

            return View(users);

        }

        public async Task<IActionResult> UserDetails(int id)
        {
            var user = await _superAdmin.GetAdminUserById(id);

            if (user == null)
            {
                _toast.AddErrorToastMessage("Invalid user");
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Block(int? userId, bool? isFromView)
        {
            if (userId is null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return NotFound();

            if (user.UserName == "superadmin")
            {
                _toast.AddErrorToastMessage("Super admin user cannot be blocked");

                return RedirectToAction(nameof(UsersList));
            }

            if (user.Status == UserStatus.Active)
            {
                user.Status = UserStatus.InActive;

                _toast.AddSuccessToastMessage("User is disabled");
            }
            else
            {
                user.Status = UserStatus.Active;

                _toast.AddSuccessToastMessage("User is enabled");
            }

            _context.Update(user);
            await _context.SaveChangesAsync();
            if (isFromView.HasValue && isFromView.Value)
            {
                return RedirectToAction(nameof(UserDetails), new { id = userId });
            }
            return RedirectToAction(nameof(UsersList));
        }

        public async Task<IActionResult> Settings()
        {
            return View();
        }

        public async Task<IActionResult> Orders()
        {
            return View(await _superAdmin.GetAllOrders());
        }

        public async Task CreateOrder(int tenantId, int planId, int userId, string userName, int duration, int employeesCount, bool isChangePlan = false)
        {
            var plan = await _context.Plans.FirstOrDefaultAsync(x => x.Id == planId);

            var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == tenantId);

            var endDate = plan.Type == PlanType.Monthly ? DateTime.Now.AddMonths(duration) : plan.Type == PlanType.Weekly ? DateTime.Now.AddDays(duration * 7) : DateTime.Now.AddYears(duration);

            string remarks = "";
            if (isChangePlan)
            {
                remarks = "A manual plan change order for " + userName + $"{tenantId}" + " of plan " + plan.Name + " " + plan.Price.ToString() + " duration of " + duration + " " + plan.Type.ToString();
            }
            else
            {
                remarks = "A manual activation order for " + userName + $"{tenantId}" + " of plan " + plan.Name + " " + plan.Price.ToString() + " duration of " + duration + " " + plan.Type.ToString();
            }

            Random random = new Random();
            var orderModel = new OrderModel
            {
                PlanId = plan.Id,
                TenantId = tenantId,
                CreatedBy = userId,
                PerEmployeeAmount = plan.Price,
                Total = plan.Price * employeesCount,
                EmployeesCount = employeesCount,
                OrderId = random.Next(00000, 99999).ToString(),
                Description = remarks,
                Status = SAOrderStatus.Success,
                PaymentGateway = PaymentGateway.Manual,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await _context.AddAsync(orderModel);
            await _context.SaveChangesAsync();

            tenant.PlanId = plan.Id;
            tenant.SubscriptionStatus = SubscriptionStatusEnum.Active;
            tenant.AvailableEmployeesCount = employeesCount;
            tenant.TotalEmployeesCount = employeesCount;
            tenant.UpdatedAt = DateTime.Now;
            _context.Tenants.Update(tenant);
            await _context.SaveChangesAsync();



            TenantSubscriptionModel subscription = new TenantSubscriptionModel
            {
                PlanId = plan.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                StartDate = DateTime.Now,
                OrderId = orderModel.Id,
                IsPromotional = plan.IsPromotional,
                CreatedBy = tenant.Id,
                TenantId = tenant.Id,
                EndDate = endDate,
            };

            await _context.AddAsync(subscription);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.Status = UserStatus.Active;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

        }

        [HttpGet]
        public IActionResult ResetPasswordBySAAdmin(int? userId)
        {

            if (userId == null) return NotFound();

            ResetPasswordVM resetPasswordVM = new ResetPasswordVM();
            resetPasswordVM.UserId = userId.Value;
            return View(resetPasswordVM);

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ResetPasswordBySAAdmin(ResetPasswordVM vm)
        {
            var user = await _context.Users.FindAsync(vm.UserId);

            if (user == null)
            {
                _toast.AddErrorToastMessage("Invalid user");
                return View(vm);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrEmpty(token))
            {
                _toast.AddErrorToastMessage("Unable to generate token");
                return View(vm);
            }

            var result = await _userManager.ResetPasswordAsync(user, token, vm.ConfirmPassword);

            if (result.Succeeded)
            {
                _toast.AddSuccessToastMessage("Password changed");

                return RedirectToAction(nameof(UsersList));
            }

            _toast.AddErrorToastMessage("Unable to change password");
            return View(vm);
        }
    }
}
