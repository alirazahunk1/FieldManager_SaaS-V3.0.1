namespace ESSWebApi.Dtos.Request.Client
{
    public class CreateUpdateRequest
    {
        public string? Name { get; set; }

        public string? Address { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ContactPerson { get; set; }

        public decimal? Radius { get; set; }

        public string? Email { get; set; }

        public string? City { get; set; }
        public string? Remarks { get; set; }

        //public string Status { get; set; }

        //public List<SignBoardRequestModel>? SignBoardRequests { get; set; }
    }
}
