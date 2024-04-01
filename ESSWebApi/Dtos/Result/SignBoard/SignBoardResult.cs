namespace ESSWebApi.Dtos.Result.SignBoard
{
    public class SignBoardResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ClientId { get; set; }

        public string Address { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public decimal Height { get; set; }

        public decimal Width { get; set; }

        public string Remarks { get; set; }

        public string CreatedAt { get; set; }
    }
}
