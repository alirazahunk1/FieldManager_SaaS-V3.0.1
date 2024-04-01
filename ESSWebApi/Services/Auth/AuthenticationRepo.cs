using ESSDataAccess.DbContext;
using ESSDataAccess.Dtos.API_Dtos.ResetPassword;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Services.Auth
{
    public class AuthenticationRepo : IAuthentication
    {
        private readonly AppDbContext _db;

        public AuthenticationRepo(AppDbContext db)
        {
            _db = db;
        }



        public async Task<bool> CheckUserName(string userName)
        {
            return await _db.Users.AsNoTracking().AnyAsync(x => x.UserName == userName);

        }

        public async Task<bool> CheckPhoneNumber(string phoneNumber)
        {
            return await _db.Users.AsNoTracking().AnyAsync(x => x.PhoneNumber == phoneNumber);

        }

        public async Task<AppUser> FindByPhoneNumber(string phoneNumber)
        {
            return await _db.Users
                .AsNoTracking()
                .Include(x => x.ResetPasswords)
                .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
        }

        public async Task<ResetPasswordDto> AddResetPassword(ResetPasswordDto data)
        {

            ResetPasswordModel result = new ResetPasswordModel
            {
                UserId = data.UserId,
                Otp = data.Otp,
                Token = data.Token,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };


            await _db.AddAsync(result);
            await _db.SaveChangesAsync();
            return data;
        }

        public async Task<ResetPasswordDto> UpdateResetPassword(ResetPasswordDto data)
        {
            ResetPasswordModel result = new ResetPasswordModel();
            result.IsVerified = data.IsVerified;
            result.UpdatedAt = DateTime.Now;

            _db.Update(result);
            await _db.SaveChangesAsync();
            return data;
        }

    }
}
