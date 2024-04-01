namespace ESSWebApi.Dtos.Request.Visit
{
    public class CreateVisitRequest
    {
        public int? ClientId { get; set; }

        public string? Remarks { get; set; }

        public string? ImgUrl { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string? Address { get; set; }
    }
}
