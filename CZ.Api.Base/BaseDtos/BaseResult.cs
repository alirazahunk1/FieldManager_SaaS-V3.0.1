using System.Text.Json.Serialization;

namespace CZ.Api.Base.BaseDtos
{
    public class BaseResult
    {
        [JsonIgnore]
        public bool IsSuccess { get; set; } = false;

        public string Message { get; set; }

        public object? Data { get; set; }
    }
}
