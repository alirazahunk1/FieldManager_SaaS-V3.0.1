using ESSWebApi.Dtos.Result;
using ESSWebApi.Manager.ManagerDtos.Requests.Expense;
using ESSWebApi.Manager.ManagerDtos.Requests.Leave;

namespace ESSWebApi.Services.Manager
{
    public interface IManager
    {
        Task<BaseResult> GetDashboardData(int userId);

        Task<BaseResult> GetEmployeeStatus(int userId);

        Task<BaseResult> GetAllLeaveRequests(int userId);

        Task<BaseResult> ChangeLeaveRequestStatus(ChangeLeaveStatus request, int userId);

        Task<BaseResult> GetAllExpenseRequests(int userId);

        Task<BaseResult> ChangeExpenseRequestStatus(ChangeExpenseStatus request, int userId);
    }
}
