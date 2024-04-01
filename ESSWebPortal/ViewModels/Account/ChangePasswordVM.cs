using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.ViewModels.Account
{
    public class ChangePasswordVM
    {
        [Required]
        [DisplayName("Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        public string Password { get; set; }

        [DisplayName("New Confirm Password")]
        [Compare("Password", ErrorMessage = "New Password and Confirmation Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
