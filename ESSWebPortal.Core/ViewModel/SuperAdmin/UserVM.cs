using ESSDataAccess.Enum;
using ESSDataAccess.Tenant.Models;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.Core.ViewModel.SuperAdmin
{
    public class UserVM
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public UserGender Gender { get; set; }

        public string? Address { get; set; }

        [Display(Name = "Registered On")]
        public DateTime CreatedAt { get; set; }

        public UserStatus Status { get; set; }

        public SubscriptionStatusEnum SubscriptionStatus { get; set; }

        public string? Avatar { get; set; }
    }
}
