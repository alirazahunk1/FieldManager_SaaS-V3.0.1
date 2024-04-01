using ESSDataAccess.Tenant;

namespace ESSWebPortal.Middleware
{
    public class MultiTenantServiceMiddleware : IMiddleware
    {
        private readonly ITenant _tenant;

        public MultiTenantServiceMiddleware(ITenant tenant)
        {
            _tenant = tenant;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string? values = string.Empty;
            if (context.Request.Cookies.TryGetValue("Tenant", out values))
            {
                if (!string.IsNullOrEmpty(values))
                {
                    //Uncomment this while running and launching migration
                    //_tenant.ClearTenant();
                    _tenant.SetTenant(Convert.ToInt32(values));
                }
            }
            await next(context);
        }
    }
}
