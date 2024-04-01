using ESSDataAccess.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.ViewModels.Account
{
    public class CreateUserVM
    {
        public int Id { get; set; }

        [Required, MinLength(6),]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Designation { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }

        public UserGender Gender { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password's length should be atleast 6")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password do not match.")]
        public string ConfirmPassword { get; set; }

        public IFormFile File { get; set; }

        public string? ImgUrl { get; set; }

    }
}
