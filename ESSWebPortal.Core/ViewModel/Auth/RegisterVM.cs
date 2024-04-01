using ESSDataAccess.Enum;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.Core.ViewModel.Auth
{
    public class RegisterVM
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, MinLength(6),]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required, Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public UserGender Gender { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password's length should be atleast 6")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
