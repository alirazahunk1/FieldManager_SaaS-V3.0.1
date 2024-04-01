using CZ.Api.Base;
using ESSCommon.Core.SMS;
using ESSDataAccess.DbContext;
using ESSDataAccess.Dtos.API_Dtos.ResetPassword;
using ESSDataAccess.Identity;
using ESSWebApi.Dtos.Request.Account;
using ESSWebApi.JWT;
using ESSWebApi.Routes;
using ESSWebApi.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Controllers
{
    [Authorize, ApiController]
    public class AuthController : BaseController
    {
        private readonly JWTManager _jWTManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;
        private readonly IAuthentication _authenticationRepo;
        private readonly ISMS _smsService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AuthController(JWTManager jWTManager,
            UserManager<AppUser> userManager,
            AppDbContext db,
            IAuthentication authentication,
            ISMS smsService,
            IWebHostEnvironment webHostEnvironment)
        {
            _jWTManager = jWTManager;
            _userManager = userManager;
            _db = db;
            _authenticationRepo = authentication;
            _smsService = smsService;
            _webHostEnvironment = webHostEnvironment;

        }

        [AllowAnonymous]
        [HttpPost(APIRoutes.Auth.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.EmployeeId))
                return BadRequest("Employee Id is required");

            if (string.IsNullOrEmpty(request.Password))
                return BadRequest("Password is required");

            if (request.EmployeeId.Length < 5)
                return BadRequest("Employee Id is invalid");

            if (request.Password.Length < 5)
                return BadRequest("Password is invalid");

            var result = await _jWTManager.Authenticate(request.EmployeeId!, request.Password!);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost(APIRoutes.Auth.ForgotPassword)]
        public async Task<IActionResult> ForgotPassword([FromBody] string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return BadRequest("Phone number is required");

            if (!await _authenticationRepo.CheckPhoneNumber(phoneNumber))
            {
                return BadRequest("User not exists");
            }

            var user = await _authenticationRepo.FindByPhoneNumber(phoneNumber.Trim());

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            Random random = new Random();
            var otp = random.Next(0000, 9999).ToString();

            await _smsService.SendSmsAsync(user.Id, $"Your otp to reset password is {otp}");

            ResetPasswordDto forgotpassword = new ResetPasswordDto
            {
                UserId = user.Id,
                Otp = otp,
                Token = token
            };

            await _authenticationRepo.AddResetPassword(forgotpassword);

            return Ok("OTP sent to phone number");

        }
        [AllowAnonymous]
        [HttpPost(APIRoutes.Auth.VerifyOtp)]
        public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPRequest request)
        {

            if (string.IsNullOrEmpty(request.PhoneNumber))
                return BadRequest("Phone number is required");

            var user = await _authenticationRepo.FindByPhoneNumber(request.PhoneNumber.Trim());
            if (user == null)
                return BadRequest("User not exists");

            if (!user.ResetPasswords.Any(x => x.CreatedAt.Date == DateTime.Now.Date))
                return BadRequest("Invalid request");

            var resetPassword = user.ResetPasswords.OrderByDescending(x => x.CreatedAt).FirstOrDefault();

            if (resetPassword.Otp != request.OTP)
                return BadRequest("Wrong OTP");


            var resetUpdate = await _db.ResetPassword.FirstOrDefaultAsync(x => x.Id == resetPassword.Id);
            resetUpdate.IsVerified = true;
            resetUpdate.UpdatedAt = DateTime.Now;
            _db.Update(resetUpdate);
            await _db.SaveChangesAsync();

            return Ok("OTP verified");
        }
        [AllowAnonymous]
        [HttpPost(APIRoutes.Auth.ResetPassword)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
                return BadRequest("Employee Id is required");

            if (string.IsNullOrEmpty(request.Password))
                return BadRequest("Password is required");

            if (request.Password.Length < 5)
                return BadRequest("Password is invalid");

            var user = await _db.Users
                .Where(x => x.PhoneNumber.Trim() == request.PhoneNumber.Trim())
                .Include(x => x.ResetPasswords)
                .FirstOrDefaultAsync();

            if (user == null)
                return BadRequest("User not exists");


            if (!user.ResetPasswords.Any(x => x.CreatedAt.Date == DateTime.Now.Date))
            {
                return BadRequest("Invalid request");
            }

            var forgot = user.ResetPasswords
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            var result = await _userManager.ResetPasswordAsync(user, forgot.Token, request.Password);
            if (!result.Succeeded)
                return BadRequest("Something went wrong");

            return Ok("Password reset successfully");
        }

        [HttpPost(APIRoutes.Auth.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(request.OldPassword))
            {
                return BadRequest("Current password is required");
            }
            else if (string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("New password is required");
            }

            if (request.NewPassword.Length < 6)
                return BadRequest("New password is invalid");

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return Unauthorized();


            if (!await _userManager.CheckPasswordAsync(user, request.OldPassword.Trim()))
                return BadRequest("Current password is invalid");

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword.Trim(),
            request.NewPassword.Trim());

            if (!result.Succeeded)
                return BadRequest("Something went wrong, please try later");


            return Ok("Password Changed");
        }


        [AllowAnonymous]
        [HttpPost(APIRoutes.Auth.CheckUsername)]
        public async Task<IActionResult> CheckUserName([FromBody] string username)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest("Employee Id is required");

            var result = await _authenticationRepo.CheckUserName(username.ToLower());

            if (!result)
                return BadRequest("User not found");


            return Ok("User exists");

        }
        [AllowAnonymous]
        [HttpPost(APIRoutes.Auth.CheckPhoneNumber)]
        public async Task<IActionResult> CheckPhoneNumber([FromBody] string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return BadRequest("Phone number is required");

            var result = await _authenticationRepo.CheckPhoneNumber(phoneNumber.Trim());

            if (!result)
                return BadRequest("Phone number not found");

            return Ok("Phone number exists");

        }
        [HttpPost(APIRoutes.Auth.UploadUserProfile)]
        public async Task<IActionResult> UploadUserProfile(IFormFile file)
        {
            var user = await _db.Users.Where(x => x.Id == GetUserId()).FirstOrDefaultAsync();

            if (user == null)
                return Unauthorized();

            if (file == null)
                return BadRequest("Invalid file");

            try
            {
                string extension = Path.GetExtension(file.FileName);
                var UniqueFileName = $@"{DateTime.Now.Ticks}" + extension;
                var filePath = $"{_webHostEnvironment.WebRootPath}\\UserProfiles\\{UniqueFileName}";

                // Full path to file in temp location
                if (file.Length > 0)
                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await file.CopyToAsync(stream);

                user.ImgUrl = "UserProfiles\\" + UniqueFileName;
                _db.Update(user);
                await _db.SaveChangesAsync();

                return Ok("user profile updated.");
            }
            catch (Exception)
            {
                return BadRequest("Something went wrong!");
            }
        }

    }
}
