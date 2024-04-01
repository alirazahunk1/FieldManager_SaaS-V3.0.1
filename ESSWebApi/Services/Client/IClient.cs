using ESSWebApi.Dtos.Request.Client;
using ESSWebApi.Dtos.Result;
using ESSWebApi.Dtos.Result.Client;

namespace ESSWebApi.Services.Client
{
    public interface IClient
    {
        Task<BaseResult> GetAll();

        Task<GetClientResult> GetAll(int skip, int take);

        Task<BaseResult> Create(CreateUpdateRequest request, int userId);

        Task<BaseResult> Update(CreateUpdateRequest request, int clientId, int userId);

        Task<BaseResult> Delete(int clientId);

        Task<BaseResult> Search(string query);
    }
}

