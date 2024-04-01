namespace ESSWebApi.Dtos.Result.Visits
{
    public class VisitHistoryDto
    {
        public int Id { get; set; }

        public string ClientName { get; set; }

        public string ClientAddress { get; set; }

        public string ClientPhoneNumber { get; set; }

        public string ClientEmail { get; set; }

        public string ClientContactPerson { get; set; }

        public string VisitImage { get; set; }

        public string VisitRemarks { get; set; }

        public string VisitDateTime { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
