using ESSWebApi.Dtos.Request.Visit;
using ESSWebApi.Dtos.Result;

namespace ESSWebApi.Services.Visit
{
    public interface IVisit
    {
        Task<BaseResult> Create(int userId, CreateVisitRequest request, IFormFile file);

        Task<BaseResult> GetHistory(int userId, DateTime date);

        Task<BaseResult> GetVisitsCount(int userId);

    }
}
