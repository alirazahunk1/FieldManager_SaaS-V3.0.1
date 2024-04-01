using ESSCommon.Core;
using ESSDataAccess;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSDataAccess.Models.Order;
using ESSDataAccess.Models.Task;
using ESSDataAccess.Tenant;
using ESSDataAccess.Tenant.Models;
using ESSWebPortal.Core.SuperAdmin;
using ESSWebPortal.Helper;
using ESSWebPortal.ViewModels.Dashboard;
using ESSWebPortal.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ESSWebPortal.Controllers
{

    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ISASettings _sASettings;
        private readonly ITenant _tenant;
        private readonly AppDbContext _context;
        private readonly IToastNotification _toast;

        private class UserLocation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
            public string Status { get; set; }
            public string UpdatedAt { get; set; }
        }

        public DashboardController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            ISASettings sASettings,
            ITenant tenant,
            AppDbContext context,
            IToastNotification toast)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _sASettings = sASettings;
            _tenant = tenant;
            _context = context;
            _toast = toast;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (await _userManager.IsInRoleAsync(user, UserRoles.SuperAdmin)) return RedirectToAction("Index", "SuperAdmin");

            //Subscription check
            if (!await _userManager.IsInRoleAsync(user, UserRoles.SuperAdmin))
            {
                var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == user.TenantId);

                if (tenant is null)
                {
                    _toast.AddErrorToastMessage("Invalid tenant data");
                }

                if (tenant.SubscriptionStatus != SubscriptionStatusEnum.Active)
                {
                    _toast.AddErrorToastMessage("Please renew your subscription");
                    return RedirectToAction("Index", "Payment");
                }

                if (!await _context.AppSettings.AnyAsync())
                {
                    _toast.AddInfoToastMessage("Please add your settings first");
                    return RedirectToAction("AddInitialSettings", "Settings");
                }
            }

            ViewData["leaveRequests"] = await _context.LeaveRequests
               .Where(x => x.Status == LeaveRequestStatus.Pending)
               .CountAsync();

            ViewData["expenseRequests"] = await _context.ExpenseRequests
                .Where(x => x.Status == ExpenseStatusEnum.Pending)
                .CountAsync();

            ViewData["formEntries"] = await _context.FormEntries
                .Where(x => x.CreatedAt.Date == DateTime.Now.Date)
                .CountAsync();

            ViewData["tasks"] = await _context.Tasks
                .Where(x => x.Status == TaskStatusEnum.InProgress || x.Status == TaskStatusEnum.Hold)
                .CountAsync();

            ViewData["orders"] = await _context.Orders
                .Where(x => x.CreatedAt.Date == DateTime.Now)
                .CountAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRecentOrdersAjax()
        {
            var orders = await _context.ProductOrders
                .Include(x => x.Client)
                .Include(x => x.User)
                .Where(x => x.Status == OrderStatus.Processing || x.Status == OrderStatus.Pending)
                .Select(x => new RecentOrderVM
                {
                    Id = x.Id,
                    ClientId = x.ClientId,
                    ClientName = x.Client.Name,
                    EmployeeId = x.UserId,
                    EmployeeName = x.User.GetFullName(),
                    ItemsCount = x.OrderLines.Count(),
                    CreatedAt = x.CreatedAt,
                    Amount = x.OrderLines.Sum(x => x.Quantity * x.Price),
                    Status = x.Status.ToString()
                }).ToListAsync();


            return Json(orders);
        }


        [HttpGet]
        public async Task<IActionResult> GetTeamWiseAttendanceAjax()
        {
            var teams = await _context.Teams
                .Select(x => new TeamModel
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .AsNoTracking()
                .ToListAsync();
            var now = DateTime.Now;
            var attendances = await _context.Attendances
                .Where(x => x.CreatedAt.Date == now.Date)
                .Include(x => x.Employee)
                .Select(x => new AttendanceModel
                {
                    Employee = new AppUser
                    {
                        TeamId = x.Employee.TeamId
                    }
                })
                .AsNoTracking()
                .ToListAsync();

            var employees = await _context.Users
                .Where(x => x.Status == UserStatus.Active && x.TenantId == _tenant.TenantId)
                .Where(x => x.TeamId != null)
                .Select(x => new AppUser
                {
                    TeamId = x.TeamId
                }).AsNoTracking()
                .ToListAsync();

            var leaveRequests = await _context.LeaveRequests
            .Where(x => now.Date >= x.FromDate.Date && now.Date <= x.ToDate.Date && x.Status == LeaveRequestStatus.Approved)
            .Include(x => x.User)
            .Select(x => new LeaveRequestModel
            {
                User = new AppUser
                {
                    TeamId = x.User.TeamId
                }
            }).AsNoTracking()
            .ToListAsync();

            List<TeamAttendanceItem> teamAttendances = new List<TeamAttendanceItem>();
            foreach (var team in teams)
            {
                var attendanceCount = attendances.Count(x => x.Employee.TeamId == team.Id);

                var onLeaveCount = leaveRequests.Count(x => x.User.TeamId == team.Id);

                TeamAttendanceItem item = new TeamAttendanceItem
                {
                    Name = team.Name,
                    Present = attendanceCount,
                    Absent = employees.Count(x => x.TeamId == team.Id) - (attendanceCount + onLeaveCount),
                    OnLeave = onLeaveCount
                };
                teamAttendances.Add(item);
            }

            return Json(teamAttendances);
        }

        [HttpGet]
        public async Task<IActionResult> GetPresentDataAjax()
        {
            var startDate = DateTime.Now.Date.AddDays(-12);

            var employeesCount = await _context.Users
                .Where(x => x.Status == UserStatus.Active && x.TenantId == _tenant.TenantId)
                .CountAsync(x => x.ScheduleId != null && x.TeamId != null);

            var attendance = await _context.Attendances
                .Where(x => x.CreatedAt.Date > startDate)
                .Select(x => new AttendanceModel
                {
                    CreatedAt = x.CreatedAt
                }).AsNoTracking()
                .ToListAsync();

            List<AttendanceStatusItem> chartItems = new List<AttendanceStatusItem>();


            for (int i = 0; i <= 12; i++)
            {
                var present = attendance.Count(x => x.CreatedAt.Date == startDate.Date);

                chartItems.Add(new AttendanceStatusItem
                {
                    Date = startDate.ToString("dd/MMM"),
                    PresentCount = present,
                    AbsentCount = employeesCount - present
                });
                startDate = startDate.AddDays(1);
            }

            return Json(chartItems);
        }

        [HttpGet]
        public async Task<IActionResult> GetTodaysAttendanceStatusAjax()
        {
            var now = DateTime.Now;

            var employeesCount = await _context.Users
                .Where(x => x.Status == UserStatus.Active && x.TenantId == _tenant.TenantId)
                .CountAsync(x => x.ScheduleId != null && x.TeamId != null);

            var presentCount = await _context.Attendances
              .Where(x => x.CreatedAt.Date == now.Date)
              .CountAsync();


            var onLeaveCount = await _context.LeaveRequests
                .Where(x => now.Date >= x.FromDate.Date && now.Date <= x.ToDate.Date && x.Status == LeaveRequestStatus.Approved)
                .CountAsync();

            return Json(new
            {
                presentCount = presentCount,
                onLeaveCount = onLeaveCount,
                absentCount = employeesCount - onLeaveCount - presentCount
            });
        }


        [HttpGet]
        public async Task<IActionResult> IndexAjax()
        {
            var result = await GetDashboardData();

            return Json(result);
        }

        public async Task<IActionResult> TimeLine()
        {
            ViewData["employees"] = await _context.Users
                .Where(x => x.ParentId != null && x.TeamId != null && x.Status == UserStatus.Active && x.TenantId == _tenant.TenantId)
                .OrderBy(x => x.FirstName)
                .Select(x => new SelectListItem { Text = x.GetFullName(), Value = x.Id.ToString() })
                .ToListAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TimeLineAjax(int? id, DateTime date)
        {
            if (id is null) return BadRequest("Employee is required");

            var emp = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (emp == null) return NotFound();

            var vm = await GetTimeLineData(emp.Id, emp.GetFullName(), date);

            return Json(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLocationAjax(int trackingId, string address)
        {
            if (string.IsNullOrEmpty(address)) return BadRequest("Address is required");

            var tracking = await _context.Trackings
                .FirstOrDefaultAsync(x => x.Id == trackingId);
            if (tracking == null) return BadRequest("Tracking not found");

            tracking.Address = address;
            tracking.UpdatedAt = DateTime.Now;
            _context.Update(tracking);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> LiveLocation()
        {
            ViewBag.employees = await _context.Users
                .Where(x => x.TeamId != null && x.ScheduleId != null && x.Status == UserStatus.Active)
                .Where(x => x.TenantId == _tenant.TenantId)
                .Select(x => new SelectListItem { Text = x.GetFullName(), Value = x.Id.ToString(), Selected = x.Id == 5 })
                .ToListAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetLiveLocationAjax()
        {
            var users = await _context.Users
                .Where(x => x.ScheduleId != null && x.TeamId != null && x.Status == UserStatus.Active)
                .Where(x => x.TenantId == _tenant.TenantId)
                .Include(x => x.UserDevice)
                .AsNoTracking()
                .Select(x => new AppUser
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserDevice = x.UserDevice
                }).ToListAsync();

            List<UserLocation> userLocations = new List<UserLocation>();
            if (users.Count() > 0)
            {
                foreach (var user in users)
                {
                    if (user.UserDevice == null)
                    {
                        continue;
                    }

                    string status = "offline";

                    var isOnline = false;

                    if (await _sASettings.GetOfflineCheckType() == "m")
                    {
                        isOnline = DateTime.Now.AddMinutes(-await _sASettings.GetOfflineCheckTime()) < user.UserDevice.UpdatedAt;
                    }
                    else
                    {
                        isOnline = DateTime.Now.AddSeconds(-await _sASettings.GetOfflineCheckTime()) < user.UserDevice.UpdatedAt;
                    }

                    if (user.UserDevice.UpdatedAt.Date == DateTime.Now.Date &&
                       isOnline)
                    {
                        status = "active";
                    }
                    else
                    {
                        status = "inactive";
                    }

                    userLocations.Add(new UserLocation
                    {
                        Id = user.Id,
                        Name = user.GetFullName(),
                        Latitude = user.UserDevice.Latitude is null ? 0 : user.UserDevice.Latitude.Value,
                        Longitude = user.UserDevice.Longitude is null ? 0 : user.UserDevice.Longitude.Value,
                        Status = status,
                        UpdatedAt = user.UserDevice.UpdatedAt.ToString("dd/MM/yyyy hh:mm:ss tt")
                    });
                }
            }

            return Json(userLocations);

        }

        [HttpGet]
        public async Task<IActionResult> CardView()
        {
            return View(await GetCardView());
        }

        [HttpGet]
        public async Task<IActionResult> CardViewAjax()
        {
            var result = await GetCardView();

            return Json(result.Users);
        }


        [HttpGet]
        public async Task<IActionResult> GetTeamWiseCount()
        {
            var now = DateTime.Now.Date;
            string key = "team_wise_view";

            CookieOptions options = new CookieOptions();
            options.Expires = now.AddDays(2);
            Response.Cookies.Append(key, "true", options);

            var teams = await _context.Teams
                .Include(x => x.Users).ThenInclude(x => x.UserDevice)
                .Select(x => new TeamModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Users = x.Users,
                })
                .AsNoTracking()
                .ToListAsync();

            var attendances = await _context.Attendances
               .Where(x => x.CreatedAt.Date == DateTime.Now.Date)
                .Select(x => new AttendanceModel
                {
                    Id = x.Id,
                    Status = x.Status,
                    CheckInTime = x.CheckInTime,
                    CheckOutTime = x.CheckOutTime,
                    EmployeeId = x.EmployeeId,
                }).AsNoTracking()
                .ToListAsync();

            List<TeamWiseCountVM> vms = new List<TeamWiseCountVM>();

            foreach (var team in teams)
            {
                TeamWiseCountVM vm = new TeamWiseCountVM
                {
                    Name = team.Name,

                };

                foreach (var user in team.Users)
                {
                    if (user.UserDevice != null)
                    {
                        if (attendances.Any(x => x.EmployeeId == user.Id))
                        {

                            var isOnline = false;

                            if (await _sASettings.GetOfflineCheckType() == "m")
                            {
                                isOnline = DateTime.Now.AddMinutes(-await _sASettings.GetOfflineCheckTime()) < user.UserDevice.UpdatedAt;
                            }
                            else
                            {
                                isOnline = DateTime.Now.AddSeconds(-await _sASettings.GetOfflineCheckTime()) < user.UserDevice.UpdatedAt;
                            }

                            if (user.UserDevice.UpdatedAt.Date == DateTime.Now.Date &&
                                 isOnline)
                            {
                                vm.OnlineCount++;
                            }
                            else if (DateTime.Now.AddMinutes(-30) < user.UserDevice.UpdatedAt)
                            {
                                vm.InActiveCount++;
                            }
                            else
                            {
                                vm.OfflineCount++;
                            }
                        }

                        else
                        {
                            vm.NotWorkingCount++;
                        }
                    }
                }
                vms.Add(vm);
            }

            return Json(vms);
        }



        public async Task<DashboardVM> GetDashboardData()
        {
            var authUser = await GetLoggedInUser();

            var users = await _context.Users
                .Where(x => x.ScheduleId != null && x.TeamId != null && x.Status == UserStatus.Active && x.TenantId == _tenant.TenantId)
                .Include(x => x.UserDevice)
                .Select(x => new AppUser
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserDevice = x.UserDevice != null ? new UserDeviceModel
                    {
                        Id = x.Id,
                        UserId = x.UserDevice.UserId,
                        UpdatedAt = x.UserDevice.UpdatedAt,
                        Latitude = x.UserDevice.Latitude,
                        Longitude = x.UserDevice.Longitude
                    } : null,
                }).AsNoTracking()
                .ToListAsync();

            var attendances = await _context.Attendances
                .Where(x => x.CreatedAt.Date == DateTime.Now.Date)
                .Select(x => new AttendanceModel
                {
                    Id = x.Id,
                    Status = x.Status,
                    CheckInTime = x.CheckInTime,
                    CheckOutTime = x.CheckOutTime,
                    EmployeeId = x.EmployeeId,
                })
                .ToListAsync();

            DashboardVM vm = new DashboardVM();
            vm.EmployeesCount = users.Count();

            vm.ExpenseRequestsCount = await _context.ExpenseRequests
                .CountAsync(x => x.Status == ExpenseStatusEnum.Pending);

            vm.LeaveRequestCount = await _context.LeaveRequests
                .CountAsync(x => x.Status == LeaveRequestStatus.Pending);

            vm.ClientsCount = await _context.Clients
                .CountAsync();

            vm.User = authUser.GetFullName();

            vm.EmployeeItems = new List<EmployeeItemVM>();
            foreach (var user in users.Where(x => x.UserDevice != null).OrderBy(x => x.FirstName))
            {
                var attendance = attendances.FirstOrDefault(x => x.EmployeeId == user.Id);

                vm.EmployeeItems.Add(new EmployeeItemVM
                {
                    Name = user.GetFullName(),
                    AttendanceInAt = attendance != null && attendance.Status == AttendanceStatus.CheckedIn ? attendance.CheckInTime.ToString("hh:mm tt") : "",
                    AttendanceOutAt = attendance != null && attendance.Status == AttendanceStatus.CheckedOut && attendance.CheckOutTime.HasValue ? attendance.CheckOutTime.Value.ToString("hh:mm tt") : "",
                    LastUpdate = user.UserDevice != null ? Helpers.ToTimeSinceString(user.UserDevice.UpdatedAt) : "No device found!",
                    Address = "",
                    Latitude = user.UserDevice != null && user.UserDevice.Latitude.HasValue ? user.UserDevice.Latitude.Value : 0,
                    Longitude = user.UserDevice != null && user.UserDevice.Longitude.HasValue ? user.UserDevice.Longitude.Value : 0
                });

                if (attendance != null)
                {
                    var isOnline = false;

                    if (await _sASettings.GetOfflineCheckType() == "m")
                    {
                        isOnline = DateTime.Now.AddMinutes(-await _sASettings.GetOfflineCheckTime()) < user.UserDevice.UpdatedAt;
                    }
                    else
                    {
                        isOnline = DateTime.Now.AddSeconds(-await _sASettings.GetOfflineCheckTime()) < user.UserDevice.UpdatedAt;
                    }

                    if (user.UserDevice.UpdatedAt.Date == DateTime.Now.Date &&
                         isOnline)
                    {
                        vm.OnlineCount++;
                    }
                    else if (DateTime.Now.AddMinutes(-30) < user.UserDevice.UpdatedAt)
                    {
                        vm.InActiveCount++;
                    }
                    else
                    {
                        vm.OfflineCount++;
                    }
                }
                else
                {
                    vm.NotWorkingCount++;
                }
            }

            return vm;
        }

        public async Task<TimeLineVM> GetTimeLineData(int userId, string name, DateTime date)
        {
            var attendance = await _context.Attendances
               .Where(x => x.CreatedAt.Date == date.Date && x.EmployeeId == userId)
               .Include(x => x.Trackings)
               .AsNoTracking()
               .FirstOrDefaultAsync();

            var device = await _context.UserDevices
                .Where(x => x.UserId == userId)
                .Select(x => new UserDeviceModel
                {
                    Brand = x.Brand,
                    Model = x.Model
                })
                .FirstOrDefaultAsync();

            TimeLineVM vm = new TimeLineVM
            {
                EmployeeName = name,
                EmployeeId = userId,
                TotalTrackedTime = "00:00",
                TotalAttendanceTime = "00:00",
                DeviceInfo = device == null ? "" : device.Brand + " " + device.Model,
                TimeLineItems = new List<TimeLineItemVM>()
            };

            if (attendance != null)
            {
                vm.AttendanceId = attendance.Id;
                if (attendance.Trackings.Count() > 0)
                {
                    TrackingFilter trackingFilter = new TrackingFilter();

                    string? preAddress = null;

                    var cIn = attendance.Trackings.FirstOrDefault();
                    var cOut = attendance.Trackings.LastOrDefault();

                    vm.TotalTrackedTime = ((cOut.CreatedAt - cIn.CreatedAt).Hours).ToString() + "  Hours";

                    vm.TotalAttendanceTime = vm.TotalTrackedTime;

                    var trackings = attendance.Trackings
                        .OrderBy(x => x.CreatedAt)
                        .Where(x => x.Accuracy > 20)
                        .DistinctBy(x => x.Latitude)
                        .DistinctBy(x => x.Longitude)
                        .DistinctBy(x => x.CreatedAt.ToString("hh:mm"))
                        .ToList();

                    //Filter by Algorithm by Anoop
                    trackings = trackingFilter.GetFilteredData(trackings);


                    //Add checkIn
                    if (!trackings.Any(x => x.Id == cIn.Id))
                    {
                        trackings.Add(cIn);
                    }
                    //AddCheckOut
                    if (!trackings.Any(x => x.Id == cOut.Id))
                    {
                        trackings.Add(cOut);
                    }


                    for (int i = 0; i < trackings.Count(); i++)
                    {
                        string elapseTime = "0";
                        var tracking = trackings[i];

                        if (tracking.Type == TrackingTypeEnum.CheckedIn)
                        {
                            if (i < trackings.Count() && trackings.Count() != 1)
                            {
                                elapseTime = GetElapseHours(tracking.CreatedAt, trackings[i + 1].CreatedAt);
                            }
                            else
                            {
                                elapseTime = "0";
                            }

                            /*   if (tracking.CreatedAt.TimeOfDay > )*/

                            vm.TimeLineItems.Add(new TimeLineItemVM
                            {
                                Id = tracking.Id,
                                Type = "checkIn",
                                Accuracy = tracking.Accuracy ?? 0,
                                Activity = tracking.Activity,
                                BatteryPercentage = tracking.BatteryPercentage,
                                IsGPSOn = tracking.IsGPSOn,
                                IsWifiOn = tracking.IsWifiOn,
                                Latitude = tracking.Latitude,
                                Longitude = tracking.Longitude,
                                Address = tracking.Address,
                                SignalStrength = tracking.SignalStrength,
                                TrackingType = tracking.Type,
                                StartTime = tracking.CreatedAt.ToString("hh:mm tt"),
                                EndTime = i + 1 < trackings.Count() ? trackings[i + 1].CreatedAt.ToString("hh:mm tt") : tracking.CreatedAt.ToString("hh:mm tt"),
                                ElapseTime = elapseTime,
                            });
                            continue;
                        }

                        if (tracking.Type == TrackingTypeEnum.CheckedOut)
                        {
                            elapseTime = tracking.CreatedAt.ToString("hh:mm tt");

                            /*   if (tracking.CreatedAt.TimeOfDay > )*/

                            vm.TimeLineItems.Add(new TimeLineItemVM
                            {
                                Id = tracking.Id,
                                Type = "checkOut",
                                Accuracy = tracking.Accuracy ?? 0,
                                Activity = tracking.Activity,
                                BatteryPercentage = tracking.BatteryPercentage,
                                IsGPSOn = tracking.IsGPSOn,
                                IsWifiOn = tracking.IsWifiOn,
                                Latitude = tracking.Latitude,
                                Longitude = tracking.Longitude,
                                Address = tracking.Address,
                                SignalStrength = tracking.SignalStrength,
                                TrackingType = tracking.Type,
                                StartTime = tracking.CreatedAt.ToString("hh:mm tt"),
                                EndTime = i + 1 < trackings.Count() ? trackings[i + 1].CreatedAt.ToString("hh:mm tt") : tracking.CreatedAt.ToString("hh:mm tt"),
                                ElapseTime = elapseTime,
                            });
                            continue;
                        }

                        if (i + 1 < trackings.Count())
                        {
                            elapseTime = GetElapseHours(tracking.CreatedAt, trackings[i + 1].CreatedAt);
                        }
                        else
                        {
                            elapseTime = tracking.CreatedAt.ToString();
                        }

                        switch (tracking.Activity)
                        {
                            case "ActivityType.STILL":
                                vm.TimeLineItems.Add(new TimeLineItemVM
                                {
                                    Id = tracking.Id,
                                    Type = "still",
                                    Accuracy = tracking.Accuracy ?? 0,
                                    Activity = tracking.Activity,
                                    BatteryPercentage = tracking.BatteryPercentage,
                                    IsGPSOn = tracking.IsGPSOn,
                                    IsWifiOn = tracking.IsWifiOn,
                                    Latitude = tracking.Latitude,
                                    Longitude = tracking.Longitude,
                                    Address = tracking.Address,
                                    SignalStrength = tracking.SignalStrength,
                                    TrackingType = tracking.Type,
                                    StartTime = tracking.CreatedAt.ToString("hh:mm tt"),
                                    EndTime = i + 1 < trackings.Count() ? trackings[i + 1].CreatedAt.ToString("hh:mm tt") : tracking.CreatedAt.ToString("hh:mm tt"),
                                    ElapseTime = elapseTime,
                                });
                                break;
                            case "ActivityType.WALKING":
                                vm.TimeLineItems.Add(new TimeLineItemVM
                                {
                                    Id = tracking.Id,
                                    Type = "walk",
                                    Accuracy = tracking.Accuracy ?? 0,
                                    Activity = tracking.Activity,
                                    BatteryPercentage = tracking.BatteryPercentage,
                                    IsGPSOn = tracking.IsGPSOn,
                                    IsWifiOn = tracking.IsWifiOn,
                                    Latitude = tracking.Latitude,
                                    Longitude = tracking.Longitude,
                                    Address = tracking.Address,
                                    SignalStrength = tracking.SignalStrength,
                                    TrackingType = tracking.Type,
                                    StartTime = tracking.CreatedAt.ToString("hh:mm tt"),
                                    EndTime = i + 1 < trackings.Count() ? trackings[i + 1].CreatedAt.ToString("hh:mm tt") : tracking.CreatedAt.ToString("hh:mm tt"),
                                    ElapseTime = elapseTime,
                                });
                                break;
                            /*  case "ActivityType.IN_VEHICLE":
                                  vm.TimeLineItems.Add(new TimeLineItemVM
                                  {

                                      Type = "vehicle",
                                      Accuracy = tracking.Accuracy ?? 0,
                                      Activity = tracking.Activity,
                                      BatteryPercentage = tracking.BatteryPercentage,
                                      IsGPSOn = tracking.IsGPSOn,
                                      IsWifiOn = tracking.IsWifiOn,
                                      Latitude = tracking.Latitude,
                                      Longitude = tracking.Longitude,
                                      Address = tracking.Address,
                                      SignalStrength = tracking.SignalStrength,
                                      TrackingType = tracking.Type,
                                      StartTime = tracking.CreatedAt.ToString("hh:mm tt"),
                                      EndTime = endTime,
                                  });
                                  break;
                              case "ActivityType.ON_BICYCLE":
                                  break;*/
                            default:
                                vm.TimeLineItems.Add(new TimeLineItemVM
                                {
                                    Id = tracking.Id,
                                    Type = "vehicle",
                                    Accuracy = tracking.Accuracy ?? 0,
                                    Activity = tracking.Activity,
                                    BatteryPercentage = tracking.BatteryPercentage,
                                    IsGPSOn = tracking.IsGPSOn,
                                    IsWifiOn = tracking.IsWifiOn,
                                    Latitude = tracking.Latitude,
                                    Longitude = tracking.Longitude,
                                    Address = tracking.Address,
                                    SignalStrength = tracking.SignalStrength,
                                    TrackingType = tracking.Type,
                                    StartTime = tracking.CreatedAt.ToString("hh:mm tt"),
                                    EndTime = i + 1 < trackings.Count() ? trackings[i + 1].CreatedAt.ToString("hh:mm tt") : tracking.CreatedAt.ToString("hh:mm tt"),
                                    ElapseTime = elapseTime,
                                    Distance = Math.Round(i + 1 < trackings.Count() ? DistanceCalculator.DistanceBetweenPlaces(Convert.ToDouble(tracking.Longitude), Convert.ToDouble(tracking.Latitude),
                                    Convert.ToDouble(trackings[i + 1].Longitude), Convert.ToDouble(trackings[i + 1].Latitude)) : 0, 2),
                                });
                                break;
                        }

                    }
                    vm.TotalKM = Math.Round(vm.TimeLineItems.Sum(x => x.Distance), 2);
                }

            }
            return vm;
        }

        private string GetElapseHours(DateTime firstTime, DateTime lastTime)
        {
            var hours = Math.Round((lastTime - firstTime).TotalHours, 2);
            return hours.ToString();
        }

        private async Task<CardViewVM> GetCardView()
        {

            CardViewVM vm = new CardViewVM();

            var teams = await _context.Teams
                .Select(x => new Team
                {
                    Name = x.Name,
                    TeamId = x.Id
                })
                .AsNoTracking()
                .ToListAsync();

            vm.Teams = teams;

            var users = await _context.Users
                .Where(x => x.ScheduleId != null && x.TeamId != null && x.Status == UserStatus.Active)
                .Where(x => x.TenantId == _tenant.TenantId)
                .Include(x => x.UserDevice)
                .Select(x => new AppUser
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    TeamId = x.TeamId,
                    UserDevice = x.UserDevice,
                }).AsNoTracking().ToListAsync();

            var attendances = await _context.Attendances
                .Where(x => x.CreatedAt.Date == DateTime.Now.Date)
                .Select(x => new AttendanceModel
                {
                    EmployeeId = x.EmployeeId,
                    CheckInTime = x.CheckInTime,
                    CheckOutTime = x.CheckOutTime
                }).AsNoTracking()
                .ToListAsync();


            foreach (var user in users)
            {
                vm.Users ??= new List<User>();

                var device = user.UserDevice;

                if (device is null) continue;

                var attendance = attendances
                    .FirstOrDefault(x => x.EmployeeId == user.Id);

                if (attendance is null) continue;

                var isOnline = false;
                var checkType = await _sASettings.GetOfflineCheckType();
                if (checkType == "m")
                {
                    isOnline = DateTime.Now.AddMinutes(-await _sASettings.GetOfflineCheckTime()) < device.UpdatedAt;
                }
                else
                {
                    isOnline = DateTime.Now.AddSeconds(-await _sASettings.GetOfflineCheckTime()) < device.UpdatedAt;
                }

                var team = teams.FirstOrDefault(x => x.TeamId == user.TeamId);

                if (team is null) continue;

                vm.Users.Add(new User
                {
                    Id = user.Id,
                    Name = user.GetFullName(),
                    BatteryLevel = device.BatteryPercentage,
                    IsGpsOn = device.IsGPSOn,
                    PhoneNumber = user.PhoneNumber,
                    SignalLevel = device.SignalStrength,
                    IsWifiOn = device.IsWifiOn,
                    UpdatedAt = device.UpdatedAt.ToString("hh:mm:ss tt"),
                    IsOnline = isOnline,
                    Latitude = device.Latitude ?? 0,
                    Longitude = device.Longitude ?? 0,
                    TeamId = team.TeamId,
                    TeamName = team.Name,
                    AttendanceInAt = attendance.CheckInTime.ToString("dd/MM/yyyy hh:mm tt"),
                    AttendanceOutAt = attendance.CheckOutTime.HasValue ? attendance.CheckOutTime.Value.ToString("dd/MM/yyyy hh:mm tt") : ""
                });
            }

            return vm;
        }


        private async Task<AppUser> GetLoggedInUser()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}
