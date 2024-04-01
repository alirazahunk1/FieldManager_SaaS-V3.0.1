using ESSDataAccess.Enum;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.ViewModels.Account
{
    public class EdtUserVM
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Designation { get; set; }

        public UserGender Gender { get; set; }

        public IFormFile? File { get; set; }

        public string? ImgUrl { get; set; }
    }
}
