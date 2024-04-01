using ESSCommon.Core.SharedModels.Settings;
using ESSDataAccess.Tenant.Models;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.ViewModels.Plan
{
    public class MyPlanVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Per Employee Price")]
        public decimal Price { get; set; }

        public PlanType Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public PlanStatus Status { get; set; } = PlanStatus.Active;

        public bool IsPromotional { get; set; } = false;

        public DateTime ExpireDate { get; set; }

        public SubscriptionStatusEnum SubscriptionStatus { get; set; }

        public int TotalEmployeesCount { get; set; }

        public int AvailableEmployeesCount { get; set; }

        public ModuleSettingsDto ModuleSettings { get; set; }
    }
}
