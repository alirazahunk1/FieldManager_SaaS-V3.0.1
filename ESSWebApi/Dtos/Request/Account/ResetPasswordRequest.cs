

namespace ESSWebApi.Dtos.Request.Account 
{ 
    public class ResetPasswordRequest
    {
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
