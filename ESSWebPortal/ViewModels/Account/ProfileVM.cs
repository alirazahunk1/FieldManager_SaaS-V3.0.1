using ESSDataAccess.Enum;

namespace ESSWebPortal.ViewModels.Account
{
    public class ProfileVM
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public UserGender Gender { get; set; }

        public DateTime? Dob { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public string? ImgUrl { get; set; }

        public UserStatus Status { get; set; }

        public string Designation { get; set; }

        public string UserName { get; set; }

    }
}
