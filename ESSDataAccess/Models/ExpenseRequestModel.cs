using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class ExpenseRequestModel : BaseModel
    {

        //TODO:Set it as a foreignkey
        public int? AttendanceId { get; set; }

        public DateTime ForDate { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public AppUser? User { get; set; }

        [Display(Name = "Approved By")]
        public int? Approvedby { get; set; }

        [Display(Name = "Approved On")]
        public DateTime? ApprovedOn { get; set; }

        [ForeignKey(nameof(ExpenseType))]
        public int ExpenseTypeId { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Approved Amount")]
        public decimal? ApprovedAmount { get; set; }

        [Display(Name = "Expense Type")]
        public ExpenseTypeModel? ExpenseType { get; set; }

        public ExpenseStatusEnum Status { get; set; }

        public string? Remarks { get; set; }

        [MaxLength(1000)]
        public string? ImgUrl { get; set; }

        [MaxLength(100)]
        public string? ApproverRemarks { get; set; }


    }
}
