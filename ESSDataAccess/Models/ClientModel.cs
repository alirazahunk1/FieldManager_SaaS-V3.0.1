using ESSDataAccess.Enum;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class ClientModel : BaseModel
    {
        [MinLength(5)]
        [MaxLength(100), Required]
        public string? Name { get; set; }
        [MaxLength(200)]
        [MinLength(25), Required]
        public string? Address { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(10, 7)")]
        public decimal Longitude { get; set; }
        [MaxLength(15)]
        [MinLength(8)]
        [Display(Name = "Phone Number"), Required]
        public string? PhoneNumber { get; set; }
        [MaxLength(100)]
        [MinLength(5)]
        [Display(Name = "Contact Person Name")]
        public string? ContactPerson { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Radius { get; set; }
        [MaxLength(50)]
        public string? Email { get; set; }
        [MaxLength(50)]
        [MinLength(5)]
        public string? City { get; set; }
        [MaxLength(250)]
        public string? Remarks { get; set; }

        [Required]
        public ClientStatus Status { get; set; }


        public List<VisitModel>? Visits { get; set; }
    }
}
