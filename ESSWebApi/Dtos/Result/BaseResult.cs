using Newtonsoft.Json;

namespace ESSWebApi.Dtos.Result
{
    public class BaseResult
    {
        [JsonIgnore]
        public bool IsSuccess { get; set; } = false;

        public string Message { get; set; }

        public object? Data { get; set; }
    }
}
