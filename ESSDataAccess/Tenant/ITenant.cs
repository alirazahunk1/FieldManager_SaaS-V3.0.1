namespace ESSDataAccess.Tenant
{
    public interface ITenant
    {
        string Tenant { get; }

        void SetTenant(string tenant);

        void SetTenant(int tenantId);

        void ClearTenant();

        int? TenantId { get; }
    }
}
