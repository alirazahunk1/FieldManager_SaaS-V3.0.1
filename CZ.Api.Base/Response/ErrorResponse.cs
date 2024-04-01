using System.Text.Json;

namespace CZ.Api.Base.Response
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }

        public string Status { get; set; }

        public object? Data { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
