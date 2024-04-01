using System.Text.Json;

namespace CZ.Api.Base.Response
{
    public class CommonResponse
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
