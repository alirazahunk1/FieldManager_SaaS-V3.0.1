using ESSWebApi.Dtos.Request.Expense;
using ESSWebApi.Dtos.Result;

namespace ESSWebApi.Services.Expense
{
    public interface IExpense
    {
        Task<BaseResult> GetExpenseTypes();

        Task<BaseResult> GetExpenseRequests(int userId);

        Task<BaseResult> CreateExpenseRequest(int userId, CreateExpenseRequest request);

        Task<BaseResult> DeleteExpenseRequest(int id);

        Task<int?> GetLastRequestId(int userId);

        Task<BaseResult> UpdateDocument(int requestId, string fileName);
    }
}
