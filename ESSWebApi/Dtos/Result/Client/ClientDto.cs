
namespace ESSWebApi.Dtos.Result.Client
{
    public class ClientDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ContactPerson { get; set; }

        public string? Email { get; set; }

        public string? City { get; set; }

        public string CreatedAt { get; set; }

        public string Status { get; set; }
    }
}
