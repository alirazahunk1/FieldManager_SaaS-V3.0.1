using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Models;
using ESSDataAccess.Tenant;
using ESSWebPortal.Core;
using ESSWebPortal.Core.Interfaces;
using ESSWebPortal.ViewModels.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Globalization;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IReport _report;
        private readonly IToastNotification _toast;
        private readonly ITenant _tenant;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(AppDbContext context,
            IReport report,
            IToastNotification toast,
            ITenant tenant,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _report = report;
            _toast = toast;
            _tenant = tenant;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Index()
        {
            ViewData["teams"] = await _context.Teams
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                }).ToListAsync();

            ViewData["users"] = await _context.Users
                .Where(x => x.Status == UserStatus.Active)
                .Where(x => x.TenantId == _tenant.TenantId)
                .Where(x => x.ScheduleId != null && x.TeamId != null)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.GetFullName(),
                }).ToListAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GenerateAttendanceReport(string type, string monthYear, List<string>? teams, List<string>? users)
        {

            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime date = DateTime.ParseExact(monthYear, "yyyy-MM", provider);

            var filePath = $"{_webHostEnvironment.WebRootPath}\\Reports\\";

            //Teams
            if (type.Equals("0"))
            {
                if (teams == null || !teams.Any())
                {
                    _toast.AddErrorToastMessage("Invalid teams selection");

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var result = await _report.GenerateAttendanceReport(filePath, type, date, teams, teams.Any(x => x == "0"));

                    if (!string.IsNullOrEmpty(result))
                    {
                        _toast.AddSuccessToastMessage("Report Generated");

                        filePath += result;

                        string fileName = Path.GetFileName(filePath);

                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                        return File(fileBytes, "application/force-download", fileName);
                    }
                }
            }
            else
            {
                _toast.AddInfoToastMessage("Under development!");

                return RedirectToAction(nameof(Index));

                if (users == null || !users.Any())
                {
                    _toast.AddErrorToastMessage("Invalid users selection");

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var result = await _report.GenerateAttendanceReport(filePath, type, date, users, users.Any(x => x == "0"));
                }
            }

            _toast.AddSuccessToastMessage("Report Generated");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AttendanceReport(DateTime? date, int? employeeId)
        {
            if (date == null)
            {
                ViewData["date"] = DateTime.Now.ToString("yyyy-MM");
            }
            else
            {
                ViewData["date"] = date.Value.ToString("yyyy-MM");
            }

            ViewData["employees"] = await _context.Users
                .Where(x => x.Status == UserStatus.Active)
                .Where(x => x.TenantId == _tenant.TenantId)
                .Where(x => x.TeamId != null && x.ScheduleId != null && x.ParentId != null)
                .OrderBy(x => x.FirstName)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.GetFullName(),
                    Selected = employeeId != null && employeeId == x.Id
                }).AsNoTracking()
                .ToListAsync();

            AttendanceRVM vm = new AttendanceRVM();
            if (employeeId != null && date != null)
            {
                var employee = await _context.Users
                    .Include(x => x.Schedule)
                    .FirstOrDefaultAsync(x => x.Id == employeeId);

                if (employee == null) return NotFound();

                vm.Name = employee.GetFullName();

                var attendances = await _context.Attendances
                    .Where(x => x.CreatedAt.Month == date.Value.Month && x.CreatedAt.Year == date.Value.Year && x.EmployeeId == employee.Id)
                    .ToListAsync();

                if (attendances.Any())
                {
                    vm.Items = new List<AttendanceReportItem>();

                    var dates = DateHelper.GetDates(date.Value.Year, date.Value.Month);

                    for (int i = 0; i < dates.Count; i++)
                    {
                        var attendance = attendances
                            .FirstOrDefault(x => x.CreatedAt.Day == dates[i].Day && x.CreatedAt.Month == dates[i].Month && x.CreatedAt.Year == dates[i].Year);

                        DateTime? checkOut = null;
                        if (attendance != null)
                        {
                            if (attendance.CheckOutTime.HasValue)
                            {
                                checkOut = attendance.CheckOutTime;
                            }
                            else
                            {
                                checkOut = await _context
                                .Trackings
                                .Where(x => x.AttendanceId == attendance.Id)
                                .OrderByDescending(x => x.CreatedAt)
                                .Select(x => x.CreatedAt)
                                .FirstOrDefaultAsync();
                            }

                            bool isWeekOff = false;


                            vm.Items.Add(new AttendanceReportItem
                            {
                                Id = i + 1,
                                Date = dates[i],
                                IsPresent = true,
                                InTime = attendance.CheckInTime.ToString("hh:mm tt"),
                                OutTime = checkOut.Value.ToString("hh:mm tt"),
                                WorkingHours = $"{(checkOut - attendance.CheckInTime).Value.Hours} Hours {(checkOut - attendance.CheckInTime).Value.Minutes} Minutes",
                                Remarks = ""
                            });
                        }
                        else
                        {
                            vm.Items.Add(new AttendanceReportItem
                            {
                                Id = i + 1,
                                Date = dates[i],
                                IsPresent = false,
                                InTime = "",
                                WorkingHours = "",
                                Remarks = IsWeekOff(employee.Schedule, dates[i]) ? "weekoff" : ""
                            });
                        }


                    }

                    return View(vm);
                }

            }

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> TimeLineReport(DateTime? date)
        {
            if (date == null)
            {
                ViewData["date"] = DateTime.Now.ToString("yyyy-MM");
            }
            else
            {
                ViewData["date"] = date.Value.ToString("yyyy-MM");
            }

            CultureInfo provider = CultureInfo.InvariantCulture;
            //DateTime date = DateTime.ParseExact(monthYear, "yyyy-MM", provider);

            var filePath = $"{_webHostEnvironment.WebRootPath}\\Reports\\";


            if (date != null)
            {
                var result = await _report.GenerateTimeLineReport(filePath, date.Value);


                if (!string.IsNullOrEmpty(result))
                {
                    _toast.AddSuccessToastMessage("Report Generated");

                    filePath = result;

                    string fileName = Path.GetFileName(filePath);

                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    return File(fileBytes, "application/force-download", fileName);
                }

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GenerateVisitReport(string visitType, string monthYear, List<string>? teams, List<string>? users)
        {

            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime date = DateTime.ParseExact(monthYear, "yyyy-MM", provider);

            var filePath = $"{_webHostEnvironment.WebRootPath}\\Reports\\Visit\\";

            //Teams
            if (visitType.Equals("1"))
            {
                if (teams == null || !teams.Any())
                {
                    _toast.AddErrorToastMessage("Invalid teams selection");

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var result = await _report.GenerateVisitReport(filePath, visitType, date, teams, teams.Any(x => x == "0"));

                    if (!string.IsNullOrEmpty(result))
                    {
                        _toast.AddSuccessToastMessage("Report Generated");

                        filePath += result;

                        string fileName = Path.GetFileName(filePath);

                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                        return File(fileBytes, "application/force-download", fileName);
                    }
                }
            }
            else if (visitType.Equals("2"))
            {
                /*_toast.AddInfoToastMessage("Under development!");

                return RedirectToAction(nameof(Index));*/

                if (users == null || !users.Any())
                {
                    _toast.AddErrorToastMessage("Invalid users selection");

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var result = await _report.GenerateVisitReport(filePath, visitType, date, users, users.Any(x => x == "0"));

                    if (!string.IsNullOrEmpty(result))
                    {
                        _toast.AddSuccessToastMessage("Report Generated");

                        filePath += result;

                        string fileName = Path.GetFileName(filePath);

                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                        return File(fileBytes, "application/force-download", fileName);
                    }
                }
            }
            else
            {
                _toast.AddWarningToastMessage("Type is required");
                return RedirectToAction(nameof(Index));
            }

            _toast.AddSuccessToastMessage("Report Generated");

            return RedirectToAction(nameof(Index));
        }

        private static bool IsWeekOff(ScheduleModel schedule, DateTime date)
        {
            var week = date.DayOfWeek;
            switch (week)
            {
                case DayOfWeek.Sunday:
                    return !schedule.Sunday;
                case DayOfWeek.Monday:
                    return !schedule.Monday;
                case DayOfWeek.Tuesday:
                    return !schedule.Tuesday;
                case DayOfWeek.Wednesday:
                    return !schedule.Wednesday;
                case DayOfWeek.Thursday:
                    return !schedule.Thursday;
                case DayOfWeek.Friday:
                    return !schedule.Friday;
                case DayOfWeek.Saturday:
                    return !schedule.Saturday;
                default:
                    break;
            }
            return false;
        }
    }
}
