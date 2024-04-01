namespace ESSWebPortal.ViewModels.Account
{
    public class RoleVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UsersCount { get; set; } = 0;
    }
}
