

namespace ESSWebApi.Dtos.Request.Device
{
    public class StatusUpdateRequest
    {
        public string? Status { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

    }
}
