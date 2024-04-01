
namespace ESSWebApi.Dtos.Result.Client
{
    public class GetClientResult : BaseResult
    {
        public List<ClientDto> Clients { get; set; }

        public int TotalCount { get; set; } = 0;
    }
}
