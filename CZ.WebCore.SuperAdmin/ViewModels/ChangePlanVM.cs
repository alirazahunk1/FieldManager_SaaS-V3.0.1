using System.ComponentModel.DataAnnotations;

namespace CZ.WebCore.SuperAdmin.ViewModels
{
    public class ChangePlanVM
    {
        public int? UserId { get; set; }

        [Display(Name = "New Plan")]
        public int? PlanId { get; set; }

        public int Duration { get; set; }

        public int TenantId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
    }
}
