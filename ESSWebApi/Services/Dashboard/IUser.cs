using ESSWebApi.Dtos.Result;

namespace ESSWebApi.Services.Dashboard
{
    public interface IUser
    {
        Task<BaseResult> GetDashboardData(int userId);
    }
}
