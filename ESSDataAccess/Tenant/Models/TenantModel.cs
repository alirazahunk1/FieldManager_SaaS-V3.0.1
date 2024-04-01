using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Tenant.Models
{
    public class TenantModel : NoTenantBaseModel
    {

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string NormalizedName { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public List<AppUser>? Users { get; set; }

        [ForeignKey(nameof(Plan))]
        public int? PlanId { get; set; }

        public PlanModel? Plan { get; set; }

        public int TotalEmployeesCount { get; set; } = 0;

        public int AvailableEmployeesCount { get; set; } = 0;

        public SubscriptionStatusEnum SubscriptionStatus { get; set; } = SubscriptionStatusEnum.OnBoarding;

        public List<TenantSubscriptionModel>? Subscriptions { get; set; }
    }

    public enum SubscriptionStatusEnum
    {
        Active,
        Expired,
        OnBoarding,
        PlanChoose,
        PaymentPending,
        PaymentFailed,
        PaymentSuccess,
        Banned
    }
}
