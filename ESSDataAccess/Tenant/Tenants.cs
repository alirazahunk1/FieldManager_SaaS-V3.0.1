namespace ESSDataAccess.Tenant
{
    public static class Tenants
    {
        public const string Internet = nameof(Internet);
        public const string Anoop = nameof(Anoop);

        public static IReadOnlyCollection<string> All = new[] { Internet, Anoop };
        public static string Find(string? value)
        {
            return All.FirstOrDefault(t => t.Equals(value?.Trim(), StringComparison.OrdinalIgnoreCase)) ?? Internet;
        }
    }
}
