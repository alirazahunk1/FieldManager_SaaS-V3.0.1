using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class SalesTargetModel : BaseModel
    {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public AppUser? User { get; set; }

        public DateTime Period { get; set; }

        [Column(TypeName = "decimal(30, 2)")]
        public decimal Target { get; set; }

        [Column(TypeName = "decimal(30, 2)")]
        public decimal Achieved { get; set; } = 0;

        [Column(TypeName = "decimal(30, 2)")]
        public decimal Balance { get; set; }

        [Column(TypeName = "decimal(2, 2)")]
        public decimal Percentage { get; set; } = 0;

        [MaxLength(1000)]
        public string? Remarks { get; set; }

        [Column(TypeName = "decimal(30, 2)")]
        public decimal? Incentive { get; set; } = 0;
    }
}
