
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.ViewModels.Expense
{
    public class ExpenseApproveVM
    {
        public int RequestId { get; set; }

        public string? ImgUrl { get; set; }

        public string EmployeeName { get; set; }

        public string CreatedAt { get; set; }

        public string ForDate { get; set; }

        public string? Remarks { get; set; }

        public bool IsConveyance { get; set; }

        public decimal ClaimedAmount { get; set; }

        public decimal ClaimedDistance { get; set; }

        [Required]
        public decimal ApprovedDistance { get; set; }

        [Required]
        public decimal ApprovedAmount { get; set; }

        public string? ApproverRemarks { get; set; }
    }
}
