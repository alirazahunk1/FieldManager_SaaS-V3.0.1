

namespace ESSWebApi.Dtos.Request.Account
{
    public class VerifyOTPRequest
    {
        public string? PhoneNumber { get; set; }
        public string? OTP { get; set; }
    }
}
