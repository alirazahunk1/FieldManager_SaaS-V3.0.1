using ESSDataAccess.Models;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Tenant.Models
{
    public class TenantSubscriptionModel : NoTenantBaseModel
    {
        [ForeignKey(nameof(Tenant))]
        public int TenantId { get; set; }
        public TenantModel? Tenant { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        public OrderModel? Order { get; set; }

        [ForeignKey(nameof(Plan))]
        public int PlanId { get; set; }

        public PlanModel? Plan { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsPromotional { get; set; } = false;

    }
}
