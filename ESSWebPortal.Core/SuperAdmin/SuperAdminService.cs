using ESSDataAccess;
using ESSDataAccess.DbContext;
using ESSDataAccess.Identity;
using ESSDataAccess.Tenant.Models;
using ESSWebPortal.Core.ViewModel.SuperAdmin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ESSWebPortal.Core.SuperAdmin
{
    public class SuperAdminService : ISuperAdmin
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SuperAdminService(AppDbContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<UserDetailsVM> GetAdminUserById(int id)
        {
            var user = await _context.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var tenant = await _context.Tenants
                .Where(x => x.Id == user!.TenantId)
                .Include(x => x.Plan)
                .Include(x => x.Subscriptions).ThenInclude(x => x.Order)
                .FirstOrDefaultAsync();

            UserDetailsVM vm = new UserDetailsVM();

            if (tenant.SubscriptionStatus != SubscriptionStatusEnum.Active)
            {
                vm.Id = id;
                vm.FirstName = user.FirstName;
                vm.LastName = user.LastName;
                vm.UserName = user.UserName;
                vm.Email = user.Email;
                vm.PhoneNumber = user.PhoneNumber;
                vm.Gender = user.Gender;
                vm.Address = user.Address;
                vm.CreatedAt = tenant.CreatedAt;
                vm.Status = user.Status;
                vm.Avatar = user.ImgUrl;
                vm.SubscriptionStatus = tenant.SubscriptionStatus;
                vm.AvailableEmployee = tenant.AvailableEmployeesCount;
                vm.TotalEmployees = tenant.TotalEmployeesCount;

            }
            else
            {
                var subscription = tenant.Subscriptions
                      .OrderByDescending(x => x.CreatedAt)
                      .FirstOrDefault();

                vm.Id = id;
                vm.FirstName = user.FirstName;
                vm.LastName = user.LastName;
                vm.UserName = user.UserName;
                vm.Email = user.Email;
                vm.PhoneNumber = user.PhoneNumber;
                vm.Gender = user.Gender;
                vm.Address = user.Address;
                vm.CreatedAt = tenant.CreatedAt;
                vm.Status = user.Status;
                vm.Avatar = user.ImgUrl;
                vm.SubscriptionStatus = tenant.SubscriptionStatus;
                vm.OrderId = subscription.Order.OrderId;
                vm.StartDate = subscription.Order.TenantSubscription.StartDate;
                vm.EndDate = subscription.Order.TenantSubscription.EndDate;
                vm.Plan = tenant.Plan.Name;
                vm.AvailableEmployee = tenant.AvailableEmployeesCount;
                vm.TotalEmployees = tenant.TotalEmployeesCount;
            }

            return vm;
        }

        public async Task<List<UserVM>> GetAdminUsers()
        {
            var admins = await _userManager.GetUsersInRoleAsync(UserRoles.Admin);



            var tenants = await _context.Tenants
                 .ToListAsync();

            List<UserVM> users = new List<UserVM>();
            foreach (var admin in admins)
            {
                users.Add(new UserVM
                {
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Id = admin.Id,
                    UserName = admin.UserName,
                    PhoneNumber = admin.PhoneNumber,
                    Email = admin.Email,
                    Gender = admin.Gender,
                    Address = admin.Address,
                    CreatedAt = admin.CreatedAt,
                    Status = admin.Status,
                    SubscriptionStatus = tenants.FirstOrDefault(x => x.Id == admin.TenantId).SubscriptionStatus,
                });
            }

            return users;
        }

        public async Task<List<OrderVM>> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(x => x.Plan)
                .Include(x => x.TenantSubscription).ThenInclude(x => x.Tenant)
                .Select(x => new OrderVM
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    Description = x.Description,
                    Tenant = x.TenantSubscription.Tenant.Name,
                    Plan = x.Plan.Name,
                    status = x.Status,
                    Amount = x.PerEmployeeAmount,
                }).ToListAsync();

            return orders;
        }
    }
}
