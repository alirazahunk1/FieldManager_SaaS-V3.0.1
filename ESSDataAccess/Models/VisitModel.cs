using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class VisitModel : BaseModel
    {
        [ForeignKey(nameof(Attendance))]
        public int? AttendanceId { get; set; }

        public AttendanceModel Attendance { get; set; }

        [ForeignKey(nameof(Client))]
        public int? ClientId { get; set; }

        public ClientModel Client { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        [Display(Name = "Image")]
        public string ImgUrl { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(10, 7)")]
        public decimal Longitude { get; set; }

        [MaxLength(1000)]
        public string? Address { get; set; }
    }
}
