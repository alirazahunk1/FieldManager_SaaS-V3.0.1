using ESSDataAccess.Enum;

namespace ESSWebApi.Dtos.Result
{
    public class LoginResult : BaseResult
    {
        public string? Token { get; set; }

        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? EmailId { get; set; }

        public string? PhoneNumber { get; set; }

        public string? EmployeeId { get; set; }
        public UserStatus Status { get; set; }
        public UserGender Gender { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string AlternatePhoneNumber { get; set; }

        public int TenantId { get; set; }
    }
}
