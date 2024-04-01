using ESSDataAccess;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSWebApi.Dtos.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ESSWebApi.JWT
{
    public class JWTManager
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public JWTManager(UserManager<AppUser> userManager,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<LoginResult> Authenticate(string employeeId, string password)
        {
            var user = await _userManager.FindByNameAsync(employeeId);
            if (user == null)
            {
                return new LoginResult
                {
                    Message = "Invalid Employee Id"
                };
            }

            bool isValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
            {
                return new LoginResult
                {
                    Message = "Wrong Password"
                };
            }

            if (user.Status != UserStatus.Active)
            {
                return new LoginResult
                {
                    Message = "User is banned, please contact administrator"
                };
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Any(x => x == UserRoles.Employee || x == UserRoles.Manager))
            {
                return new LoginResult
                {
                    Message = "You don't have access, please contact administrator"
                };
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("id",user.Id.ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);

            return new LoginResult
            {
                IsSuccess = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                EmployeeId = user.UserName,
                EmailId = user.Email,
                PhoneNumber = user.PhoneNumber,
                Status = user.Status,
                Gender = user.Gender,
                Avatar = user.ImgUrl,
                AlternatePhoneNumber = user.AlternatePhoneNumber,
                Address = user.Address,
                TenantId = user.TenantId.Value,
            };
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMonths(6),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
