
using ESSDataAccess.DbContext;
using Microsoft.EntityFrameworkCore;

namespace ESSCommon.Core.Subscription
{
    public class SubscriptionService : ISubscription
    {
        private readonly AppDbContext _context;

        public SubscriptionService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<bool> IsSubscriptionActive(int tenantId)
        {

            var tenant = await _context.Tenants
                .Include(x => x.Subscriptions)
                .Where(x => x.Id == tenantId)
                .FirstOrDefaultAsync();

            if (tenant == null) return false;

            if (tenant.Subscriptions == null || !tenant.Subscriptions.Any()) return false;

            var lastSubscription = tenant.Subscriptions
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            if (lastSubscription == null) return false;

            if (DateTime.Now > lastSubscription.EndDate)
            {
                tenant.SubscriptionStatus = ESSDataAccess.Tenant.Models.SubscriptionStatusEnum.Expired;

                _context.Tenants.Update(tenant);
                await _context.SaveChangesAsync();
                return false;
            }

            return true;
        }
    }
}
