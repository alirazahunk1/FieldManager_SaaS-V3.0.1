using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Tenant.Models
{
    public class InvoiceModel : NoTenantBaseModel
    {
        [ForeignKey(nameof(Tenant))]
        public int TenantId { get; set; }

        public TenantModel? Tenant { get; set; }

        public string? InvoiceNumber { get; set; }

        public DateTime Date { get; set; }

        public DateTime DueDate { get; set; }

        public int EmployeesCount { get; set; }

        [ForeignKey(nameof(Plan))]
        public int PlanId { get; set; }

        public PlanModel? Plan { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal PerEmployeeAmount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Discount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Total { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Tax { get; set; }

        [MaxLength(300)]
        public string? Remarks { get; set; }

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

    }

    public enum InvoiceStatus
    {
        Pending,
        Paid,
        OverDue,
        Cancelled
    }
}
