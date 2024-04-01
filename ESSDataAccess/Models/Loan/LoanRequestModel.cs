using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Loan
{
    public class LoanRequestModel : BaseModel
    {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public AppUser? User { get; set; }

        [MaxLength(100)]
        public string Remarks { get; set; }

        public LoanStatus Status { get; set; } = LoanStatus.Pending;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? ApprovedAmount { get; set; }

        public int? ApprovedById { get; set; }

        [MaxLength(100)]
        [Display(Name = "Approver Remarks")]
        public string? ApproverRemarks { get; set; }


        public DateTime? ActionTakenOn { get; set; }
    }

    public enum LoanStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }
}
