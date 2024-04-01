using ESSDataAccess.Models.Base;
using ESSDataAccess.Tenant.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class OrderModel : NoTenantBaseModel
    {
        [MaxLength(20)]
        public string OrderId { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        public int TenantId { get; set; }

        [ForeignKey(nameof(Plan))]
        public int PlanId { get; set; }

        public PlanModel? Plan { get; set; }

        public int EmployeesCount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal PerEmployeeAmount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Discount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Total { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Tax { get; set; }


        public string? PaymentResponse { get; set; }

        public PaymentGateway PaymentGateway { get; set; }

        public SAOrderStatus Status { get; set; } = SAOrderStatus.Created;

        public TenantSubscriptionModel? TenantSubscription { get; set; }
    }

    public enum SAOrderStatus
    {
        Created,
        Pending,
        Success,
        Failed,
        FailedWException
    }
}
