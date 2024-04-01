using ESSWebPortal.Core.ViewModel.SuperAdmin;

namespace ESSWebPortal.Core.SuperAdmin
{
    public interface ISuperAdmin
    {
        Task<List<UserVM>> GetAdminUsers();

        Task<UserDetailsVM> GetAdminUserById(int id);

        Task<List<OrderVM>> GetAllOrders();
    }
}
