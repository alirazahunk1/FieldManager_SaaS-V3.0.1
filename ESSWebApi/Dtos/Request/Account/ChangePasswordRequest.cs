
namespace ESSWebApi.Dtos.Request.Account
{
    public class ChangePasswordRequest
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
