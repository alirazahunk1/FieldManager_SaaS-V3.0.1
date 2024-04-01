using ESSCommon.Core;
using ESSCommon.Core.Settings;
using ESSDataAccess;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSDataAccess.Models.Qr;
using ESSDataAccess.Tenant;
using ESSWebPortal.ViewModels.Account;
using ESSWebPortal.ViewModels.Device;
using ESSWebPortal.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITenant _tenant;
        private readonly IDbSettings _dbSettings;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private class Location
        {
            public string Text { get; set; }

            public decimal Latitude { get; set; }

            public decimal Longitude { get; set; }
        }
        public EmployeeController(AppDbContext context,
            UserManager<AppUser> userManager,
            ITenant tenant,
            IDbSettings dbSettings,
            IWebHostEnvironment hostingEnvironment,
            IToastNotification toast) : base(toast)
        {
            _context = context;
            _userManager = userManager;
            _tenant = tenant;
            _dbSettings = dbSettings;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {

            var teams = await _context.Teams
                .Select(x => new TeamModel { Id = x.Id, Name = x.Name })
                .ToListAsync();

            var employees = await _context.Users
                .Where(x => x.ScheduleId != null && x.TeamId != null)
                .Where(x => x.TenantId == _tenant.TenantId)
                .Select(x => new AppUser
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Address = x.Address,
                    Email = x.Email,
                    Gender = x.Gender,
                    ParentId = x.ParentId,
                    PhoneNumber = x.PhoneNumber,
                    Status = x.Status,
                    UserName = x.UserName,
                    ImgUrl = x.ImgUrl,
                    TeamId = x.TeamId,
                    AttendanceType = x.AttendanceType,
                }).ToListAsync();


            List<EmployeeVM> employeesVMs = new List<EmployeeVM>();

            int activeCount = 0, inactiveCount = 0;
            foreach (var emp in employees)
            {
                if (emp.Status == UserStatus.Active)
                {
                    activeCount++;
                }
                else
                {
                    inactiveCount++;
                }

                employeesVMs.Add(new EmployeeVM
                {
                    Id = emp.Id,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Address = emp.Address,
                    Email = emp.Email,
                    Gender = emp.Gender,
                    ParentId = emp.ParentId,
                    PhoneNumber = emp.PhoneNumber,
                    Status = emp.Status,
                    UserName = emp.UserName,
                    ImgUrl = emp.ImgUrl,
                    TeamId = emp.TeamId,
                    EmployeeRestrictionType = emp.AttendanceType,
                    TeamName = teams.FirstOrDefault(x => x.Id == emp.TeamId).Name
                });
            }

            ViewData["activecount"] = activeCount;

            ViewData["inactivecount"] = inactiveCount;

            ViewData["totalcount"] = activeCount + inactiveCount;

            return View(employeesVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Block(int? userId, bool? isFromView)
        {
            if (userId is null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return NotFound();

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
                return RedirectToAction(nameof(View), new { userId = userId });
            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Create()
        {
            ViewData["schedules"] = await GetSchedulesSelectList();

            ViewData["managers"] = await GetManagersSelectList();

            ViewData["teams"] = await GetTeamsSelectList();

            var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == _tenant.TenantId);

            if (tenant == null || tenant.AvailableEmployeesCount <= 0)
            {
                ToastErrorMessage("You have reached the maximum limit of employees");
                return RedirectToAction(nameof(Index));
            }

            var tenantEmployeesCount = await _context.Users
                .CountAsync(x => x.TenantId == _tenant.TenantId && x.ScheduleId != null && x.TeamId != null);

            if (tenantEmployeesCount >= tenant.AvailableEmployeesCount)
            {
                ToastErrorMessage("You have reached the maximum limit of employees");
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> View(int? userId)
        {
            if (userId is null) return NotFound();

            var user = await _context.Users
                .Where(x => x.Id == userId)
                .Include(x => x.Team)
                .Include(x => x.Schedule)
                .Include(x => x.UserDevice)
                .Include(x => x.QrCodeGroup)
                .Include(x => x.IpGroup)
                .Include(x => x.GeofenceGroup)
                .Include(x => x.Site)
                .Select(x => new EmpVM
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    BaseSalary = x.BaseSalary.HasValue ? x.BaseSalary.Value : 0,
                    AlternatePhoneNumber = x.AlternatePhoneNumber,
                    Dob = x.Dob.HasValue ? x.Dob.Value.ToString(SharedConstants.DateFormat) : "",
                    DateOfJoining = x.DateOfJoining.HasValue ? x.DateOfJoining.Value.ToString(SharedConstants.DateFormat) : "",
                    Gender = x.Gender,
                    ParentId = x.ParentId,
                    Status = x.Status,
                    TeamId = x.TeamId,
                    Address = x.Address,
                    AvailableLeaveCount = x.AvailableLeaveCount,
                    ImgUrl = x.ImgUrl,
                    PrimarySalesTarget = x.PrimarySalesTarget != null ? x.PrimarySalesTarget : 0,
                    SecondarySalesTarget = x.SecondarySalesTarget != null ? x.SecondarySalesTarget : 0,
                    Designation = x.Designation,
                    UniqueId = x.UniqueId,
                    ScheduleId = x.ScheduleId,
                    Schedule = x.Schedule.Title,
                    TeamName = x.Team.Name,
                    AttendanceType = x.AttendanceType.Value,
                    GeofenceGroupId = x.GeofenceGroupId,
                    GeofenceGroupName = x.GeofenceGroup != null ? x.GeofenceGroup.Name : "",
                    IpGroupId = x.IpGroupId,
                    IpGroupName = x.IpGroup != null ? x.IpGroup.Name : "",
                    QrGroupId = x.QrCodeGroupId,
                    QrGroupName = x.QrCodeGroup != null ? x.QrCodeGroup.Name : "",
                    SiteId = x.SiteId,
                    SiteName = x.Site != null ? x.Site.Name : "",
                    DynamicQrId = x.DynamicQrId,
                    DynamicQrName = x.DynamicQr != null ? x.DynamicQr.Name : "",
                    Device = x.UserDevice != null ? new DeviceVM
                    {
                        Address = x.UserDevice.Address,
                        BatteryPercentage = x.UserDevice.BatteryPercentage,
                        Board = x.UserDevice.Board,
                        Brand = x.UserDevice.Brand,
                        DeviceId = x.UserDevice.DeviceId,
                        DeviceType = x.UserDevice.DeviceType,
                        Model = x.UserDevice.Model,
                        IsGPSOn = x.UserDevice.IsGPSOn,
                        LastUpdatedOn = x.UserDevice.UpdatedAt,
                        SdkVersion = x.UserDevice.SdkVersion,
                        Latitude = x.UserDevice.Latitude,
                        Longitude = x.UserDevice.Longitude
                    } : null
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null) return NotFound();

            var parentName = await _context.Users
                .Where(x => x.Id == user.ParentId)
                .Select(x => x.UserName)
                .FirstOrDefaultAsync();

            user.ParentName = parentName != null ? parentName : "";

            ViewData["ipGroups"] = await _context.IpGroups
                .Where(x => x.Status == CommonStatus.Active)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

            ViewData["geofenceGroups"] = await _context.GeofenceGroups
                .Where(x => x.Status == CommonStatus.Active)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

            ViewData["qrGroups"] = await _context.QrCodeGroups
                .Where(x => x.Status == CommonStatus.Active)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();



            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) return NotFound();

            List<Location> locations = new List<Location>();
            var attendance = await _context.Attendances
                .Where(x => x.CreatedAt.Date == DateTime.Now.Date && x.EmployeeId == user.Id)
                .Include(x => x.Trackings)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (attendance == null || attendance.Trackings.Count() <= 0) return NotFound();

            foreach (var item in attendance.Trackings)
            {
                string type = "";
                switch (item.Type)
                {
                    case TrackingTypeEnum.CheckedIn:
                        type = "Check In";
                        break;
                    case TrackingTypeEnum.Travelling:
                        type = "Travelling";
                        break;
                    case TrackingTypeEnum.Still:
                        type = "Still";
                        break;
                    case TrackingTypeEnum.CheckedOut:
                        type = "Check Out";
                        break;
                    case TrackingTypeEnum.AutoCheckedOut:
                        type = "Auto Check Out";
                        break;
                    default:
                        break;
                }
                locations.Add(new Location
                {
                    Text = $"{type} ({item.CreatedAt.ToString(SharedConstants.TimeFormat)})",
                    Latitude = item.Latitude,
                    Longitude = item.Longitude
                });
            }

            return Json(locations);
        }

        [HttpGet]
        public async Task<IActionResult> GetGeofenceGroups()
        {
            var groups = await _context.GeofenceGroups
                .Where(x => x.Status == CommonStatus.Active)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

            return Json(groups);
        }

        [HttpGet]
        public async Task<IActionResult> GetIpGroups()
        {
            var groups = await _context.IpGroups
                .Where(x => x.Status == CommonStatus.Active)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

            return Json(groups);
        }

        [HttpGet]
        public async Task<IActionResult> GetQrGroups()
        {
            var groups = await _context.QrCodeGroups
                .Where(x => x.Status == CommonStatus.Active)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

            return Json(groups);
        }

        [HttpGet]
        public async Task<IActionResult> GetDynamicQrs()
        {
            var qrs = await _context.DynamicQrs
                .Where(x => x.Status == DynamicQrStatus.New)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

            return Json(qrs);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDynamicQrs()
        {
            var qrs = await _context.DynamicQrs
                .Where(x => x.Status != DynamicQrStatus.Deactivated)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

            return Json(qrs);
        }

        [HttpGet]
        public async Task<IActionResult> GetSites()
        {
            var qrs = await _context.Sites
                .Where(x => x.Status == CommonStatus.Active)
                .Where(x => x.IsAttendanceEnabled)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

            return Json(qrs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUpdateEmployeeVM vm)
        {
            if (ModelState.IsValid)
            {
                vm.UserName = vm.UserName.Trim();
                vm.Email = vm.Email.Trim();
                vm.PhoneNumber = vm.PhoneNumber.Trim();
                if (await _context.Users.AnyAsync(x => x.UserName.ToLower() == vm.UserName.ToLower()))
                {
                    ViewData["schedules"] = await GetSchedulesSelectList(vm.ScheduleId);

                    ViewData["managers"] = await GetManagersSelectList(vm.ParentId);

                    ViewData["teams"] = await GetTeamsSelectList(vm.TeamId);

                    ToastErrorMessage("User name already exists");

                    return View(vm);
                }

                if (await _context.Users.AnyAsync(x => x.PhoneNumber.ToLower() == vm.PhoneNumber.ToLower()))
                {
                    ViewData["schedules"] = await GetSchedulesSelectList(vm.ScheduleId);

                    ViewData["managers"] = await GetManagersSelectList(vm.ParentId);

                    ViewData["teams"] = await GetTeamsSelectList(vm.TeamId);

                    ToastErrorMessage("Phone number already exists");

                    return View(vm);
                }

                if (await _context.Users.AnyAsync(x => x.Email.ToLower() == vm.Email.ToLower()))
                {
                    ViewData["schedules"] = await GetSchedulesSelectList(vm.ScheduleId);

                    ViewData["managers"] = await GetManagersSelectList(vm.ParentId);

                    ViewData["teams"] = await GetTeamsSelectList(vm.TeamId);

                    ToastErrorMessage("Email already exists");

                    return View(vm);
                }

                if (vm.ParentId == 0 || vm.TeamId == 0 || vm.ScheduleId == 0)
                {
                    ViewData["schedules"] = await GetSchedulesSelectList(vm.ScheduleId);

                    ViewData["managers"] = await GetManagersSelectList(vm.ParentId);

                    ViewData["teams"] = await GetTeamsSelectList(vm.TeamId);

                    ToastErrorMessage("Invalid work details");

                    return View(vm);
                }

                ViewData["schedules"] = await GetSchedulesSelectList(vm.ScheduleId);

                ViewData["managers"] = await GetManagersSelectList(vm.ParentId);

                ViewData["teams"] = await GetTeamsSelectList(vm.TeamId);

                var moduleSettings = await _dbSettings.GetModuleSettings();

                if (vm.AttendanceType == EmployeeAttendanceTypeEnum.Ip)
                {
                    if (!moduleSettings.IsIpBasedAttendanceModuleEnabled)
                    {
                        ToastErrorMessage("Ip based attendance module is not enabled");

                        return View(vm);
                    }

                    if (vm.IpGroupId == null)
                    {
                        ToastErrorMessage("Invalid Ip Group");

                        return View(vm);
                    }
                }
                else if (vm.AttendanceType == EmployeeAttendanceTypeEnum.Geofence)
                {
                    if (!moduleSettings.IsGeofenceModuleEnabled)
                    {
                        ToastErrorMessage("Geofence based attendance module is not enabled");

                        return View(vm);
                    }

                    if (vm.GeofenceGroupId == null)
                    {
                        ToastErrorMessage("Invalid Geofence Group");

                        return View(vm);
                    }
                }
                else if (vm.AttendanceType == EmployeeAttendanceTypeEnum.StaticQRCode)
                {
                    if (!moduleSettings.IsQrCodeAttendanceModuleEnabled)
                    {
                        ToastErrorMessage("Qr code based attendance module is not enabled");

                        return View(vm);
                    }

                    if (vm.QrGroupId == null)
                    {
                        ToastErrorMessage("Invalid Qr Group");

                        return View(vm);
                    }
                }
                else if (vm.AttendanceType == EmployeeAttendanceTypeEnum.DynamicQRCode)
                {
                    if (!moduleSettings.IsDynamicQrCodeAttendanceEnabled)
                    {
                        ToastErrorMessage("Dynamic Qr code based attendance module is not enabled");

                        return View(vm);
                    }

                    if (vm.DynamicQrId == null)
                    {
                        ToastErrorMessage("Invalid Dynamic Qr");

                        return View(vm);
                    }
                }
                else if (vm.AttendanceType == EmployeeAttendanceTypeEnum.Site)
                {
                    if (!moduleSettings.IsSiteModuleEnabled)
                    {
                        ToastErrorMessage("Site based attendance module is not enabled");

                        return View(vm);
                    }

                    if (vm.SiteId == null)
                    {
                        ToastErrorMessage("Invalid Site");

                        return View(vm);
                    }
                }

                var userId = _userManager.GetUserId(User);

                if (userId == null) return NotFound();

                AppUser user = new AppUser
                {
                    UserName = vm.UserName,
                    NormalizedUserName = vm.UserName.ToUpper(),
                    FirstName = vm.FirstName ?? "",
                    LastName = vm.LastName ?? "",
                    PhoneNumber = vm.PhoneNumber,
                    UniqueId = vm.UniqueId,
                    Address = vm.Address,
                    AlternatePhoneNumber = vm.AlternateNumber,
                    BaseSalary = vm.BaseSalary,
                    CreatedBy = Convert.ToInt32(userId),
                    DateOfJoining = vm.DateOfJoin,
                    Designation = vm.Designation ?? "",
                    Dob = vm.Dob,
                    Email = vm.Email,
                    NormalizedEmail = vm.Email.ToUpper(),
                    Gender = vm.Gender,
                    ParentId = vm.ParentId,
                    TeamId = vm.TeamId,
                    ScheduleId = vm.ScheduleId,
                    PrimarySalesTarget = vm.PrimarySalesTarget,
                    SecondarySalesTarget = vm.SecondarySalesTarget,
                    AvailableLeaveCount = vm.AvailableLeaveCount,
                    AttendanceType = vm.AttendanceType,
                    TenantId = _tenant.TenantId,
                    IpGroupId = vm.IpGroupId,
                    GeofenceGroupId = vm.GeofenceGroupId,
                    DynamicQrId = vm.DynamicQrId,
                    SiteId = vm.SiteId,
                    QrCodeGroupId = vm.QrGroupId,
                    ImgUrl = vm.File != null ? await SaveProfilePicture(vm.UserName, vm.File) : null,
                };

                try
                {
                    var result = await _userManager.CreateAsync(user, vm.ConfirmPassword);

                    if (result.Succeeded)
                    {
                        var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == _tenant.TenantId);

                        if (tenant != null)
                        {
                            tenant.AvailableEmployeesCount -= 1;
                            _context.Update(tenant);
                            await _context.SaveChangesAsync();
                        }

                        await _userManager.AddToRoleAsync(user, UserRoles.Employee);


                    }
                    else
                    {
                        ViewData["schedules"] = await GetSchedulesSelectList(vm.ScheduleId);

                        ViewData["managers"] = await GetManagersSelectList(vm.ParentId);

                        ViewData["teams"] = await GetTeamsSelectList(vm.TeamId);

                        ToastErrorMessage(result.Errors.ToString() ?? "Failed Error");

                        string errors = "";

                        foreach (var error in result.Errors)
                        {
                            errors += "Code:" + error.Code + "\n Description:" + error.Description + "\n";
                        }

                        ViewData["error"] = errors;

                        return View(vm);
                    }

                }
                catch (Exception)
                {
                    ViewData["schedules"] = await GetSchedulesSelectList(vm.ScheduleId);

                    ViewData["managers"] = await GetManagersSelectList(vm.ParentId);

                    ViewData["teams"] = await GetTeamsSelectList(vm.TeamId);

                    ToastErrorMessage("Unable to create employee");

                    return View(vm);
                }

                ToastSuccessMessage("Employee successfully added");

                return RedirectToAction(nameof(Index));
            }

            ViewData["schedules"] = await GetSchedulesSelectList(vm.ScheduleId);

            ViewData["managers"] = await GetManagersSelectList(vm.ParentId);

            ViewData["teams"] = await GetTeamsSelectList(vm.TeamId);

            ToastValidationErrorMsg();

            return View(vm);
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

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Where(x => x.Id == id)
                .Select(x => new UpdateEmployeeVM
                {
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    UniqueId = x.UniqueId,
                    Address = x.Address,
                    AlternateNumber = x.AlternatePhoneNumber,
                    BaseSalary = x.BaseSalary ?? 0,
                    DateOfJoin = x.DateOfJoining ?? DateTime.Now,
                    Designation = x.Designation,
                    Dob = x.Dob ?? DateTime.Now,
                    Email = x.Email,
                    Gender = x.Gender,
                    ParentId = x.ParentId.Value,
                    TeamId = x.TeamId.Value,
                    ScheduleId = x.ScheduleId.Value,
                    PrimarySalesTarget = x.PrimarySalesTarget,
                    SecondarySalesTarget = x.SecondarySalesTarget,
                    AvailableLeaveCount = x.AvailableLeaveCount,
                    ImgUrl = x.ImgUrl,
                    AttendanceType = x.AttendanceType.Value,
                    DynamicQrId = x.DynamicQrId,
                    GeofenceGroupId = x.GeofenceGroupId,
                    IpGroupId = x.IpGroupId,
                    QrGroupId = x.QrCodeGroupId,
                    SiteId = x.SiteId,
                }).FirstOrDefaultAsync();

            if (user == null)
            {
                ToastErrorMessage("User not found");

                return NotFound();
            }



            ViewData["schedules"] = await GetSchedulesSelectList(user.ScheduleId);

            ViewData["managers"] = await GetManagersSelectList(user.ParentId);

            ViewData["teams"] = await GetTeamsSelectList(user.TeamId);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateEmployeeVM vm)
        {
            ViewData["schedules"] = await GetSchedulesSelectList(vm.ScheduleId);

            ViewData["managers"] = await GetManagersSelectList(vm.ParentId);

            ViewData["teams"] = await GetTeamsSelectList(vm.TeamId);

            if (ModelState.IsValid)
            {
                var moduleSettings = await _dbSettings.GetModuleSettings();

                if (vm.AttendanceType == EmployeeAttendanceTypeEnum.Ip)
                {
                    if (!moduleSettings.IsIpBasedAttendanceModuleEnabled)
                    {
                        ToastErrorMessage("Ip based attendance module is not enabled");

                        return View(vm);
                    }

                    if (vm.IpGroupId == null)
                    {
                        ToastErrorMessage("Invalid Ip Group");

                        return View(vm);
                    }
                }
                else if (vm.AttendanceType == EmployeeAttendanceTypeEnum.Geofence)
                {
                    if (!moduleSettings.IsGeofenceModuleEnabled)
                    {
                        ToastErrorMessage("Geofence based attendance module is not enabled");

                        return View(vm);
                    }

                    if (vm.GeofenceGroupId == null)
                    {
                        ToastErrorMessage("Invalid Geofence Group");

                        return View(vm);
                    }
                }
                else if (vm.AttendanceType == EmployeeAttendanceTypeEnum.StaticQRCode)
                {
                    if (!moduleSettings.IsQrCodeAttendanceModuleEnabled)
                    {
                        ToastErrorMessage("Qr code based attendance module is not enabled");

                        return View(vm);
                    }

                    if (vm.QrGroupId == null)
                    {
                        ToastErrorMessage("Invalid Qr Group");

                        return View(vm);
                    }
                }
                else if (vm.AttendanceType == EmployeeAttendanceTypeEnum.DynamicQRCode)
                {
                    if (!moduleSettings.IsDynamicQrCodeAttendanceEnabled)
                    {
                        ToastErrorMessage("Dynamic Qr code based attendance module is not enabled");

                        return View(vm);
                    }

                    if (vm.DynamicQrId == null)
                    {
                        ToastErrorMessage("Invalid Dynamic Qr");

                        return View(vm);
                    }
                }
                else if (vm.AttendanceType == EmployeeAttendanceTypeEnum.Site)
                {
                    if (!moduleSettings.IsSiteModuleEnabled)
                    {
                        ToastErrorMessage("Site based attendance module is not enabled");

                        return View(vm);
                    }

                    if (vm.SiteId == null)
                    {
                        ToastErrorMessage("Invalid Site");

                        return View(vm);
                    }
                }


                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == vm.Id);


                if (user.UserName != vm.UserName.Trim() && await _context.Users.AnyAsync(x => x.UserName.ToLower() == vm.UserName.ToLower().Trim() && x.Id != vm.Id))
                {
                    ToastErrorMessage("User name already exists");

                    return View(vm);
                }

                if (user.PhoneNumber != vm.PhoneNumber.Trim() && await _context.Users.AnyAsync(x => x.PhoneNumber.ToLower() == vm.PhoneNumber.ToLower().Trim() && x.Id != vm.Id))
                {
                    ToastErrorMessage("Phone number already exists");

                    return View(vm);
                }

                user.UserName = vm.UserName.Trim().ToLower();
                user.NormalizedUserName = vm.UserName.Trim().ToUpper();
                user.FirstName = vm.FirstName;
                user.LastName = vm.LastName;
                user.PhoneNumber = vm.PhoneNumber;
                user.UniqueId = vm.UniqueId;
                user.Address = vm.Address;
                user.AlternatePhoneNumber = vm.AlternateNumber;
                user.BaseSalary = vm.BaseSalary;
                user.DateOfJoining = vm.DateOfJoin;
                user.Designation = vm.Designation;
                user.Dob = vm.Dob;
                user.Email = vm.Email.Trim().ToLower();
                user.NormalizedEmail = vm.Email.ToUpper();
                user.Gender = vm.Gender;
                user.ParentId = vm.ParentId;
                user.TeamId = vm.TeamId;
                user.ScheduleId = vm.ScheduleId;
                user.PrimarySalesTarget = vm.PrimarySalesTarget;
                user.SecondarySalesTarget = vm.SecondarySalesTarget;
                user.AvailableLeaveCount = vm.AvailableLeaveCount;
                user.AttendanceType = vm.AttendanceType;
                user.IpGroupId = vm.IpGroupId;
                user.GeofenceGroupId = vm.GeofenceGroupId;
                user.DynamicQrId = vm.DynamicQrId;
                user.SiteId = vm.SiteId;
                user.QrCodeGroupId = vm.QrGroupId;
                user.SiteId = vm.SiteId;


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

                return RedirectToAction(nameof(Index));
            }


            ToastValidationErrorMsg();

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Where(x => x.Id == id)
                .Select(x => new CreateUpdateEmployeeVM
                {
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    UniqueId = x.UniqueId,
                    Address = x.Address,
                    AlternateNumber = x.AlternatePhoneNumber,
                    BaseSalary = x.BaseSalary ?? 0,
                    DateOfJoin = x.DateOfJoining ?? DateTime.Now,
                    Designation = x.Designation,
                    Dob = x.Dob ?? DateTime.Now,
                    Email = x.Email,
                    Gender = x.Gender,
                    ParentId = x.ParentId.Value,
                    TeamId = x.TeamId.Value,
                    ScheduleId = x.ScheduleId.Value,
                    PrimarySalesTarget = x.PrimarySalesTarget.Value,
                    SecondarySalesTarget = x.SecondarySalesTarget.Value,
                    AvailableLeaveCount = x.AvailableLeaveCount
                }).FirstOrDefaultAsync();

            if (user == null)
            {
                ToastErrorMessage("User not found");

                return NotFound();
            }

            return View(user);
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

                return RedirectToAction(nameof(Index));
            }

            ToastErrorMessage("Unable to change password");
            return View(vm);
        }


        private async Task<List<SelectListItem>> GetTeamsSelectList()
        {
            return await _context.Teams
                  .Select(x => new SelectListItem
                  {
                      Value = x.Id.ToString(),
                      Text = x.Name
                  }).ToListAsync();
        }

        private async Task<List<SelectListItem>> GetManagersSelectList()
        {
            var managers = await _userManager
                .GetUsersInRoleAsync(UserRoles.Manager);

            return managers.Where(x => x.TenantId == _tenant.TenantId)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.GetFullName()
                }).ToList();
        }

        private async Task<List<SelectListItem>> GetSchedulesSelectList()
        {
            return await _context.Schedules
                 .Select(x => new SelectListItem
                 {
                     Value = x.Id.ToString(),
                     Text = x.Title
                 }).ToListAsync();
        }

        private async Task<List<SelectListItem>> GetTeamsSelectList(int id)
        {
            return await _context.Teams
                  .Select(x => new SelectListItem
                  {
                      Value = x.Id.ToString(),
                      Text = x.Name,
                      Selected = x.Id == id
                  }).ToListAsync();
        }

        private async Task<List<SelectListItem>> GetManagersSelectList(int id)
        {
            var managers = await _userManager.GetUsersInRoleAsync(UserRoles.Manager);

            return managers.Where(x => x.TenantId == _tenant.TenantId)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.GetFullName(),
                    Selected = x.Id == id
                }).ToList();
        }

        private async Task<List<SelectListItem>> GetSchedulesSelectList(int id)
        {
            return await _context.Schedules
                 .Select(x => new SelectListItem
                 {
                     Value = x.Id.ToString(),
                     Text = x.Title,
                     Selected = x.Id == id
                 }).ToListAsync();
        }


    }
}
