using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.Models.Auth
{
    public class LoginViewModel
    {
        [Required]
        [MinLength(5), MaxLength(50)]
        [Display(Name = "Email")]
        public string? UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
