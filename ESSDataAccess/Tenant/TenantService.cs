namespace ESSDataAccess.Tenant
{
    public class TenantService : ITenant
    {

        public int? TenantId { get; private set; }

        public string Tenant { get; private set; } = Tenants.Internet;

        public void ClearTenant()
        {
            TenantId = null;
        }

        public void SetTenant(string tenant)
        {
            Tenant = tenant;
        }

        public void SetTenant(int tenantId)
        {
            TenantId = tenantId;
        }
    }
}
