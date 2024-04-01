using ESSWebApi.Dtos.Request.Leave;
using ESSWebApi.Dtos.Result;
using ESSWebApi.Dtos.Result.Leave;

namespace ESSWebApi.Services.Leave
{
    public interface ILeave
    {
        Task<List<LeaveTypeDto>> GetLeaveTypes();

        Task<BaseResult> GetLeaveRequests(int userId);

        Task<BaseResult> CreateLeaveRequest(int userId, CreateLeaveRequest request);

        Task<int?> GetLastRequestId(int userId);

        Task UpdateDocument(int requestId, string documentUrl);

        Task DeleteRequest(int requestId);

    }
}
