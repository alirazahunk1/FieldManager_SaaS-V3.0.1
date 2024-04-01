using ESSDataAccess.Enum;
using ESSDataAccess.Tenant.Models;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.Core.ViewModel.SuperAdmin
{
    public class UserDetailsVM
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public UserGender Gender { get; set; }

        public string? Address { get; set; }

        [Display(Name = "Registered On")]
        public DateTime CreatedAt { get; set; }

        public UserStatus Status { get; set; }

        public string? Avatar { get; set; }

        //Subscription Details
        public SubscriptionStatusEnum SubscriptionStatus { get; set; }

        public string? OrderId { get; set; }

        public DateTime? StartDate { get; set; }

        [Display(Name = "Expires On")]
        public DateTime? EndDate { get; set; }

        public string? Plan { get; set; }

        public int TotalEmployees { get; set; }

        public int AvailableEmployee { get; set; }
    }
}
