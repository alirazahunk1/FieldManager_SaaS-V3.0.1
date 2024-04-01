using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CZ.WebCore.SuperAdmin.ViewModels
{
    public class ResetPasswordVM
    {
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Password), MinLength(6)]
        [DisplayName("New Password")]
        public string Password { get; set; }

        [DisplayName("New Confirm Password")]
        [Compare("Password", ErrorMessage = "New Password and Confirmation Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
