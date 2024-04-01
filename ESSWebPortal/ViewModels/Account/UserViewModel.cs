using ESSDataAccess.Enum;

namespace ESSWebPortal.ViewModels.Account
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string UserName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public string? Designation { get; set; }

        public string PhoneNumber { get; set; }

        public int? ChildCount { get; set; }

        public UserGender Gender { get; set; }

        public UserStatus Status { get; set; }

        public string Email { get; set; }

        public string ImgUrl { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
