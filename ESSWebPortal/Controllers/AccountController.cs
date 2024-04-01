using ESSDataAccess;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSDataAccess.Tenant;
using ESSWebPortal.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITenant _tenant;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IToastNotification toast,
            IWebHostEnvironment hostingEnvironment,
            ITenant tenant,
            AppDbContext context) : base(toast)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvironment;
            _tenant = tenant;
            _context = context;
        }



        [HttpPost]
        public async Task<IActionResult> SavePushToken(string? token)
        {
            if (string.IsNullOrEmpty(token)) return BadRequest("Token is required");

            var userId = _userManager.GetUserId(User);

            var authUserId = string.IsNullOrEmpty(userId) ? 0 : Convert.ToInt32(userId);

            if (authUserId == 0) return BadRequest("Invalid user");

            try
            {
                var userToken = await _context.PushTokens.FirstOrDefaultAsync(x => x.UserId == authUserId);

                if (userToken is null)
                {
                    PushTokenModel pushToken = new PushTokenModel
                    {
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Token = token,
                        Type = PushTokenType.Web,
                        CreatedBy = authUserId,
                        UserId = authUserId
                    };

                    await _context.AddAsync(pushToken);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    userToken.Token = token;
                    userToken.UpdatedBy = authUserId;
                    userToken.UpdatedAt = DateTime.Now;

                    _context.Update(userToken);
                    await _context.SaveChangesAsync();
                }

                return Ok("Successfully saved");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            ProfileVM vm = new ProfileVM()
            {
                Id = user.Id,
                DateOfJoining = user.DateOfJoining,
                Designation = user.Designation,
                Dob = user.Dob,
                FirstName = user.FirstName,
                Gender = user.Gender,
                ImgUrl = user.ImgUrl,
                LastName = user.LastName,
                Status = user.Status,
                UserName = user.UserName,
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> UsersList()
        {
            return View(await GetUsersAsync());
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ResetPasswordByAdmin(int? userId)
        {

            if (userId == null) return NotFound();

            ResetPasswordVM resetPasswordVM = new ResetPasswordVM();
            resetPasswordVM.UserId = userId.Value;
            return View(resetPasswordVM);

        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ResetPasswordByAdmin(ResetPasswordVM vm)
        {
            var user = await _context.Users.FindAsync(vm.UserId);

            if (user == null)
            {
                ToastErrorMessage("Invalid user");
                return View(vm);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrEmpty(token))
            {
                ToastErrorMessage("Unable to generate token");
                return View(vm);
            }

            var result = await _userManager.ResetPasswordAsync(user, token, vm.ConfirmPassword);

            if (result.Succeeded)
            {
                ToastSuccessMessage("Password changed");

                return RedirectToAction(nameof(UsersList));
            }

            ToastErrorMessage("Unable to change password");
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> DeletePortalUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);


            if (user == null) return NotFound();

            if (user.UserName == "adminuser")
            {
                ToastErrorMessage("Unable to delete super admin");
            }
            else
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();

                ToastSuccessMessage("User deleted");
            }


            return RedirectToAction(nameof(UsersList));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (Constants.IsDemoMode)
                {
                    if (user.UserName.Equals("adminuser"))
                    {
                        ToastErrorMessage("You're not allowed to change the admin password");

                        return RedirectToAction("Index", "Dashboard");
                    }
                }


                if (!await _userManager.CheckPasswordAsync(user, vm.OldPassword))
                {
                    ToastErrorMessage("Old password is incorrect");

                    return View(vm);
                }

                var result = await _userManager.ChangePasswordAsync(user, vm.OldPassword, vm.ConfirmPassword);

                if (!result.Succeeded)
                {
                    ToastErrorMessage("Unable to change password");

                    return View(vm);
                }

                ToastSuccessMessage("Password changed");

                return RedirectToAction(nameof(UsersList));
            }

            ToastErrorMessage("Validation error");

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Roles()
        {
            var roles = await _context.Roles
                .Select(x => new RoleVM
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToListAsync();
            return View(roles);
        }


        public async Task<IActionResult> Create()
        {
            ViewData["roles"] = await GetRolesSelectList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserVM vm)
        {
            if (ModelState.IsValid)
            {
                vm.UserName = vm.UserName.Trim();
                vm.PhoneNumber = vm.PhoneNumber.Trim();
                vm.Email = vm.Email.Trim();

                if (await _context.Users.AnyAsync(x => x.UserName.ToLower() == vm.UserName.ToLower()))
                {
                    ViewData["roles"] = await GetRolesSelectList(vm.RoleId);
                    ToastErrorMessage("User name already exists");
                    return View(vm);
                }

                if (await _context.Users.AnyAsync(x => x.PhoneNumber.ToLower() == vm.PhoneNumber.ToLower()))
                {
                    ViewData["roles"] = await GetRolesSelectList(vm.RoleId);
                    ToastErrorMessage("Phone number already exists");
                    return View(vm);
                }

                if (await _context.Users.AnyAsync(x => x.Email.ToLower() == vm.Email.ToLower()))
                {
                    ViewData["roles"] = await GetRolesSelectList(vm.RoleId);
                    ToastErrorMessage("Email already exists");
                    return View(vm);
                }

                AppUser user = new AppUser
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    UserName = vm.UserName,
                    NormalizedUserName = vm.UserName.ToUpper(),
                    Email = vm.Email,
                    PhoneNumber = vm.PhoneNumber,
                    Gender = vm.Gender,
                    TenantId = _tenant.TenantId,
                    Designation = vm.Designation,
                    ImgUrl = vm.File != null ? await SaveProfilePicture(vm.UserName, vm.File) : null,
                };

                try
                {
                    var result = await _userManager.CreateAsync(user, vm.ConfirmPassword);

                    if (result.Succeeded)
                    {
                        var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == vm.RoleId);

                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else
                    {
                        ViewData["roles"] = await GetRolesSelectList(vm.RoleId);

                        ToastErrorMessage("Unable to create user identity error");

                        return View(vm);
                    }

                    ToastSuccessMessage("User successfully created");

                    return RedirectToAction(nameof(UsersList));
                }
                catch (Exception)
                {
                    ViewData["roles"] = await GetRolesSelectList(vm.RoleId);

                    ToastErrorMessage("Unable to create user");

                    return View(vm);
                }

            }

            ViewData["roles"] = await GetRolesSelectList(vm.RoleId);

            ToastValidationErrorMsg();

            return View(vm);

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Where(x => x.Id == id)
                .Select(x => new EdtUserVM
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Designation = x.Designation,
                    Email = x.Email,
                    Gender = x.Gender,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    ImgUrl = x.ImgUrl
                }).FirstOrDefaultAsync();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EdtUserVM vm)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(x => x.UserName.ToLower() == vm.UserName.ToLower() && x.Id != vm.Id))
                {

                    ToastErrorMessage("User name already exists");

                    return View(vm);
                }

                if (await _context.Users.AnyAsync(x => x.PhoneNumber.ToLower() == vm.PhoneNumber.ToLower() && x.Id != vm.Id))
                {

                    ToastErrorMessage("Phone number already exists");

                    return View(vm);
                }

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == vm.Id);
                user.UserName = vm.UserName.ToLower();
                user.NormalizedUserName = vm.UserName.ToUpper();
                user.FirstName = vm.FirstName;
                user.LastName = vm.LastName;
                user.PhoneNumber = vm.PhoneNumber;
                user.Designation = vm.Designation;
                user.Email = vm.Email.ToLower();
                user.NormalizedEmail = vm.Email.ToUpper();
                user.Gender = vm.Gender;

                if (vm.File != null)
                {
                    user.ImgUrl = await SaveProfilePicture(vm.UserName, vm.File);
                }


                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();


                }
                catch (Exception)
                {
                    ToastErrorMessage("Unable to update employee");

                    return View(vm);
                }

                ToastSuccessMessage("Employee successfully updated");

                return RedirectToAction(nameof(UsersList));
            }

            ToastValidationErrorMsg();

            return View(vm);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context == null)
            {
                return NotFound();
            }

            var vm = await GetUserById(id.Value);

            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        public async Task<IActionResult> DeleteUser(int? id)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Block(int? userId, bool? isFromView)
        {
            if (userId is null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return NotFound();

            if (user.UserName == "adminuser")
            {
                ToastErrorMessage("Admin user cannot be blocked");

                return RedirectToAction(nameof(UsersList));
            }

            if (user.Status == UserStatus.Active)
            {
                user.Status = UserStatus.InActive;

                ToastSuccessMessage("User is disabled");
            }
            else
            {
                user.Status = UserStatus.Active;

                ToastSuccessMessage("User is enabled");
            }

            _context.Update(user);
            await _context.SaveChangesAsync();
            if (isFromView.HasValue && isFromView.Value)
            {
                return RedirectToAction(nameof(Details), new { id = userId });
            }
            return RedirectToAction(nameof(UsersList));
        }

        private async Task<UserViewModel> GetUserById(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            var role = await _userManager.GetRolesAsync(user);

            UserViewModel vm = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Designation = user.Address,
                Gender = user.Gender,
                ChildCount = user.ParentId,
                PhoneNumber = user.PhoneNumber,
                Role = role.FirstOrDefault(),
                UserName = user.UserName,
                Email = user.Email,
                Status = user.Status
            };

            return vm;
        }

        private async Task<List<UserViewModel>> GetUsersAsync()
        {
            IList<AppUser> users = await _context.Users
                    .Where(x => x.TenantId == _tenant.TenantId)
                    .Where(x => x.ScheduleId == null && x.TeamId == null)
                    .ToListAsync();


            List<UserViewModel> vms = new List<UserViewModel>();

            foreach (var user in users)
            {
                var role = _userManager.GetRolesAsync(user).Result.First();

                vms.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Designation = user.Designation,
                    Gender = user.Gender,
                    ChildCount = users.Count(x => x.ParentId == user.Id),
                    PhoneNumber = user.PhoneNumber,
                    Role = role,
                    Status = user.Status,
                    Email = user.Email,
                });
            }

            return vms;
        }

        private async Task<List<SelectListItem>> GetRolesSelectList()
        {
            return await _context.Roles
                .Where(x => x.Name.ToLower() != UserRoles.Employee && x.Name.ToLower() != UserRoles.SuperAdmin)
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToListAsync();
        }

        private async Task<List<SelectListItem>> GetRolesSelectList(int id)
        {
            return await _context.Roles
                .Where(x => x.Name.ToLower() != UserRoles.Employee && x.Name.ToLower() != UserRoles.SuperAdmin)
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == id
                }).ToListAsync();
        }

        private async Task<string> SaveProfilePicture(string userName, IFormFile file)
        {
            string folderName = "Upload/Profile/";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            string extention = Path.GetExtension(file.FileName);
            string fileName = userName + DateTime.Now.Ticks + extention;
            string fullPath = Path.Combine(newPath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return folderName + fileName;
        }
    }
}
