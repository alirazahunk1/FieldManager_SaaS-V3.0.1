using System.Text.Json;

namespace ESSWebApi.Dtos.Response
{
    public class Response
    {
        public int StatusCode { get; set; }

        public ResponseStatus Status { get; set; }

        public object? Data { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public enum ResponseStatus
    {
        success,
        error
    }
}
