using ESSDataAccess.Dtos.API_Dtos.MessagingToken;
using ESSDataAccess.Dtos.API_Dtos.ResetPassword;
using ESSDataAccess.Identity;

namespace ESSWebApi.Services.Auth
{
    public interface IAuthentication
    {
        Task<bool> CheckUserName(string userName);
        Task<bool> CheckPhoneNumber(string phoneNumber);
        Task<AppUser> FindByPhoneNumber(string phoneNumber);
        Task<ResetPasswordDto> AddResetPassword(ResetPasswordDto data);
        Task<ResetPasswordDto> UpdateResetPassword(ResetPasswordDto data);
    }
}
