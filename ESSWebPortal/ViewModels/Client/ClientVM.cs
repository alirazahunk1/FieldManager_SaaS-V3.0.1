using ESSDataAccess.Enum;
using ESSDataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.ViewModels.Client
{
    public class ClientVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Contact Person")]
        public string? ContactPerson { get; set; }

        public decimal? Radius { get; set; }

        public string? Email { get; set; }

        public string? City { get; set; }

        public string? Remarks { get; set; }

        public ClientStatus Status { get; set; }

        public List<VisitModel>? Visits { get; set; }

        public string? RequestedOn { get; set; }
    }
}
