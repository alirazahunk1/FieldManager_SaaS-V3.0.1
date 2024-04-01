using CZ.Api.Base;
using ESSWebApi.Routes;
using ESSWebApi.Services.Dashboard;
using ESSWebApi.Services.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESSWebApi.Controllers
{
    [Authorize, ApiController]
    public class UserController : BaseController
    {
        private readonly IScheduleManager _schedule;
        private readonly IUser _userRepo;

        public UserController(
            IScheduleManager scheduleManager,
            IUser userRepo)
        {

            _schedule = scheduleManager;
            _userRepo = userRepo;
        }

        [HttpGet(APIRoutes.User.GetDashboardData)]
        public async Task<IActionResult> GetDashboardData()
        {
            var result = await _userRepo.GetDashboardData(GetUserId());
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }


        /*  [HttpGet(APIRoutes.Attendance.StatusChecking)]
          public async Task<IActionResult> StatusChecking()
          {

              var attendanceStatus = await _attendance.GetAttendanceStatus(HttpContext.GetUserId());
              return Ok(attendanceStatus);
          }*/


        [HttpGet(APIRoutes.Attendance.GetSchedule)]
        public async Task<IActionResult> GetSchedule()
        {
            var result = await _schedule.GetUserSchedule(GetUserId());

            return Ok(result);
        }
    }
}
