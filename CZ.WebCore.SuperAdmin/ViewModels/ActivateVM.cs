using System.ComponentModel.DataAnnotations;

namespace CZ.WebCore.SuperAdmin.ViewModels
{
    public class ActivateVM
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public int TenantId { get; set; }

        [Display(Name = "Select Plan")]
        public int PlanId { get; set; }

        public int Duration { get; set; }

        public int EmployeesCount { get; set; }
    }
}
