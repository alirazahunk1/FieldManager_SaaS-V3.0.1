using CZ.Api.Base;
using CZ.Api.Base.Extensions;
using ESSCommon.Core;
using ESSCommon.Core.Services.Notification;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSDataAccess.Models.Site;
using ESSWebApi.Dtos.Request.Attendance;
using ESSWebApi.Dtos.Result.Attendance;
using ESSWebApi.Dtos.Result.Device;
using ESSWebApi.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ESSWebApi.Controllers
{
    [Authorize, ApiController]
    public class AttendanceController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly INotification _notification;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AttendanceController(AppDbContext context,
            INotification notification,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost(APIRoutes.Attendance.PostProof)]
        public async Task<IActionResult> PostProof(IFormFile file)
        {
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(x => x.EmployeeId == GetUserId() && x.CreatedAt.Date == DateTime.Now.Date);

            if (attendance == null)
                return BadRequest("No attendance found");

            if (file == null)
                return BadRequest("Invalid file");

            try
            {
                string extension = Path.GetExtension(file.FileName);
                var UniqueFileName = $@"{attendance.Id}_{DateTime.Now.Ticks}" + extension;
                var filePath = $"{_webHostEnvironment.WebRootPath}\\AttendanceImages\\{UniqueFileName}";

                // Full path to file in temp location
                if (file.Length > 0)
                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await file.CopyToAsync(stream);
                TrackingModel tracking = new()
                {
                    AttendanceId = attendance.Id,
                    Type = TrackingTypeEnum.ProofPost,
                    ImageUrl = UniqueFileName,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await _context.AddAsync(tracking);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest("Something went wrong while saving file");
            }


            return Ok("Updated");
        }

        [HttpPost(APIRoutes.Attendance.StatusUpdate)]
        public async Task<IActionResult> AttendanceStatusUpdate([FromBody] UpdateStatusRequest request)
        {
            if (request.Latitude == null || request.Longitude == null) return BadRequest("Location is required");


            var attendance = await _context.Attendances!
                .Where(x => x.EmployeeId == HttpContext.GetUserId() && x.CreatedAt.Date == DateTime.Now.Date)
                .AsNoTracking()
                .Select(x => new AttendanceModel { Id = x.Id })
                .FirstOrDefaultAsync();

            if (attendance == null) return BadRequest("CheckIn first");

            TrackingModel tracking = new TrackingModel
            {
                AttendanceId = attendance.Id,
                Accuracy = request.Accuracy,
                Activity = request.Activity,
                BatteryPercentage = request.BatteryPercentage!.Value,
                IsGPSOn = request.IsGPSOn,
                IsWifiOn = request.IsWifiOn,
                Latitude = request.Latitude.Value,
                Longitude = request.Longitude.Value,
                IsMock = request.IsMock,
                Type = request.Status.Equals("still") ? TrackingTypeEnum.Still : TrackingTypeEnum.Travelling,
                SignalStrength = request.SignalStrength,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            try
            {
                await _context.AddAsync(tracking);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Unable to update status");
            }

            return Ok("Status updated");
        }

        [HttpPost(APIRoutes.Attendance.CheckInOut)]
        public async Task<IActionResult> CheckInOut([FromBody] CheckInOutRequest request)
        {
            if (string.IsNullOrEmpty(request.Status))
                return BadRequest("Status is required");

            if (!request.Status.Equals("checkin") && !request.Status.Equals("checkout"))
                return BadRequest("Invalid status");

            if (request.Latitude == null || request.Longitude == null)
                return BadRequest("Location is required");

            var userId = HttpContext.GetUserId();
            var now = DateTime.Now;
            //TODO: Duplicate request validation
            var attendance = await _context.Attendances
                    .FirstOrDefaultAsync(x => x.CreatedAt.Date == DateTime.Now.Date && x.EmployeeId == userId);

            if (attendance != null && request.Status.Equals("checkin"))
                return BadRequest("Already checked in");

            if (attendance == null && request.Status.Equals("checkout"))
                return BadRequest("Check in first");

            if (request.Status.Equals("checkin"))
            {
                if (attendance == null)
                {

                    AttendanceModel att = new AttendanceModel
                    {
                        EmployeeId = userId,
                        CreatedAt = now,
                        UpdatedAt = now,
                        CheckInTime = now,
                        Status = AttendanceStatus.CheckedIn
                    };

                    try
                    {
                        await _context.AddAsync(att);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        return BadRequest("Db insert failed");
                    }


                    TrackingModel tracking = new TrackingModel
                    {
                        Accuracy = 100,
                        AttendanceId = att.Id,
                        CreatedAt = now,
                        UpdatedAt = now,
                        BatteryPercentage = request.BatteryPercentage.Value,
                        IsGPSOn = request.IsLocationOn,
                        IsWifiOn = request.IsWifiOn,
                        Latitude = request.Latitude.Value,
                        Longitude = request.Longitude.Value,
                        IsMock = request.IsMock,
                        SignalStrength = request.SignalStrength,
                        Type = TrackingTypeEnum.CheckedIn
                    };

                    try
                    {
                        await _context.AddAsync(tracking);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        return BadRequest("Db insert failed");
                    }

                    await _notification.PostCheckInOut(userId, AttendanceNotificationType.CheckIn);

                }
            }
            else
            {
                if (attendance != null)
                {
                    attendance.Status = AttendanceStatus.CheckedOut;
                    attendance.CheckOutTime = attendance.UpdatedAt = now;

                    _context.Update(attendance);
                    await _context.SaveChangesAsync();

                    TrackingModel tracking = new TrackingModel
                    {
                        Accuracy = 100,
                        AttendanceId = attendance.Id,
                        CreatedAt = now,
                        UpdatedAt = now,
                        BatteryPercentage = request.BatteryPercentage.Value,
                        IsGPSOn = request.IsLocationOn,
                        IsWifiOn = request.IsWifiOn,
                        Latitude = request.Latitude.Value,
                        Longitude = request.Longitude.Value,
                        IsMock = request.IsMock,
                        SignalStrength = request.SignalStrength,
                        Type = TrackingTypeEnum.CheckedOut
                    };

                    try
                    {
                        await _context.AddAsync(tracking);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        return BadRequest("Db insert failed");
                    }

                    await _notification.PostCheckInOut(userId, AttendanceNotificationType.CheckOut);
                }
            }

            return Ok("Status updated");
        }

        [HttpGet(APIRoutes.Attendance.StatusChecking)]
        public async Task<IActionResult> CheckStatus()
        {
            var userId = GetUserId();
            StatusUpdateResult result = new StatusUpdateResult();

            var user = await _context.Users
                .Where(x => x.Id == userId)
                .Include(x => x.Site)
                .Select(x => new AppUser
                {
                    Status = x.Status,
                    AttendanceType = x.AttendanceType,
                    Site = new SiteModel
                    {
                        Name = x.Site.Name,
                        AttendanceType = x.Site.AttendanceType,
                    }
                }).FirstOrDefaultAsync();

            result.UserStatus = user.Status.ToString().ToLower();

            var attendance = await _context.Attendances
                .Include(x => x.Employee).ThenInclude(x => x.Schedule)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EmployeeId == userId && x.CreatedAt.Date == DateTime.Now.Date);

            result.Status = "new";
            if (attendance != null)
            {
                if (attendance.Status == AttendanceStatus.CheckedIn)
                {
                    result.Status = "checkedin";
                    result.CheckInAt = attendance.CheckInTime.ToString(SharedConstants.TimeFormat);

                    result.TravelledDistance = await GetTravelledDistance(attendance.Id);

                    var now = DateTime.Now;

                    result.TrackedHours = (now - attendance.CheckInTime).Hours;
                }
                else
                {
                    result.Status = "checkedout";
                    result.CheckInAt = attendance.CheckInTime.ToString(SharedConstants.TimeFormat);
                    result.CheckOutAt = attendance.CheckOutTime != null ? attendance.CheckOutTime.Value.ToString(SharedConstants.TimeFormat) : "";
                }
            }

            var shift = attendance != null ? attendance.Employee.Schedule : await _context.Users
                   .Include(x => x.Schedule)
                   .Where(x => x.Id == userId)
                   .Select(x => x.Schedule)
                   .FirstOrDefaultAsync();

            //Populate shift info
            result.ShiftStartTime = shift.StartTime.ToString(SharedConstants.TimeFormat);

            result.ShiftEndTime = shift.EndTime.ToString(SharedConstants.TimeFormat);

            //Late Checking
            if (result.Status == "new")
            {
                result.IsLate = shift.StartTime.TimeOfDay < DateTime.Now.TimeOfDay;
            }
            else
            {
                result.IsLate = shift.StartTime.TimeOfDay < attendance.CheckInTime.TimeOfDay;
            }

            //On Leave Checking
            result.IsOnLeave = await _context.LeaveRequests
                .Where(x => x.UserId == userId && x.Status == LeaveRequestStatus.Approved)
                .Where(x => x.FromDate.Date <= DateTime.Now.Date && x.ToDate.Date >= DateTime.Now.Date)
                .AnyAsync();

            if (attendance != null)
            {
                var breakStatus = await _context.Breaks
               .FirstOrDefaultAsync(x => x.AttendanceId == attendance.Id && x.EndTime == null);


                result.IsOnBreak = breakStatus != null ? true : false;


                result.BreakStartedAt = breakStatus != null ? breakStatus.StartTime.ToString(SharedConstants.TimeFormatWS) : null;
            }

            result.AttendanceType = user.AttendanceType.ToString().ToLower();


            if (user.AttendanceType == EmployeeAttendanceTypeEnum.Site && user.Site != null)
            {
                result.SiteAttendanceType = user.Site.AttendanceType.ToString().ToLower();
                result.AttendanceType = result.SiteAttendanceType;
                result.SiteName = user.Site.Name;
                result.IsSiteEmployee = true;
            }

            return Ok(result);

        }


        [HttpGet(APIRoutes.Attendance.GetAttendanceHistory)]
        public async Task<IActionResult> GetHistory(int month, int year)
        {

            var attendance = await _context.Attendances
                .Where(x => x.EmployeeId == HttpContext.GetUserId() && x.CreatedAt.Date.Month == month && x.CreatedAt.Date.Year == year)
                .Select(x => new AttendanceDto
                {
                    Id = x.Id,
                    Date = x.CreatedAt.Date.ToString("dd/MM/yyyy"),
                    CheckInTime = x.CheckInTime.ToString("hh:mm tt"),
                    CheckOutTime = x.CheckOutTime.HasValue ? x.CheckOutTime.Value.ToString("hh:mm tt") : string.Empty,
                    Status = x.Status.ToString(),
                    ApprovedOn = x.ApprovedOn.HasValue ? x.ApprovedOn.Value.ToString("dd/MM/yyyy hh:mm tt") : string.Empty,
                })
                .ToListAsync();

            return Ok(attendance);
        }

        [HttpGet(APIRoutes.Attendance.CanCheckOut)]
        public async Task<IActionResult> CanCheckOut()
        {
            var attendance = await _context.Attendances
               .Include(x => x.Employee).ThenInclude(x => x.Schedule)
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.EmployeeId == GetUserId() && x.CreatedAt.Date == DateTime.Now.Date);

            if (attendance == null)
            {
                return BadRequest("No attendance found");
            }

            var now = DateTime.Now;

            var shift = attendance.Employee.Schedule;

            if (shift != null)
            {
                if (shift.EndTime.TimeOfDay < now.TimeOfDay)
                {
                    return Ok("You can check out");
                }
                else
                {
                    return BadRequest("You can't check out");
                }
            }
            else
            {
                return BadRequest("No shift found");
            }
        }

        [HttpPost(APIRoutes.Attendance.SetEarlyCheckoutReason)]
        public async Task<IActionResult> SetEarlyCheckoutReason([FromBody] string reason)
        {
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(x => x.EmployeeId == HttpContext.GetUserId() && x.CreatedAt.Date == DateTime.Now.Date);

            if (attendance == null)
                return BadRequest("No attendance found");

            attendance.EarlyCheckOutReason = reason;
            _context.Update(attendance);
            await _context.SaveChangesAsync();

            return Ok("Updated");
        }

        private async Task<decimal> GetTravelledDistance(int attendanceId)
        {
            try
            {
                var trackingData = await _context.Trackings
                    .Where(x => x.AttendanceId == attendanceId)
                    .Where(x => x.Latitude != 0 || x.Longitude != 0)
                    .OrderBy(x => x.CreatedAt)
                    .AsNoTracking()
                    .ToListAsync();

                if (trackingData.Count == 0 || trackingData.Count == 1)
                {
                    return 0;
                }

                var currentTracking = trackingData.FirstOrDefault();

                double totalDistance = 0;

                for (int i = 0; i < trackingData.Count; i++)
                {
                    if (i == trackingData.Count - 1)
                    {
                        break;
                    }

                    var nextTracking = trackingData[i + 1];

                    totalDistance += SharedHelper
                        .DistanceTo(Convert.ToDouble(currentTracking.Latitude), Convert.ToDouble(currentTracking.Longitude),
                        Convert.ToDouble(nextTracking.Latitude), Convert.ToDouble(nextTracking.Longitude));

                    currentTracking = nextTracking;
                }

                return Convert.ToDecimal(totalDistance);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
