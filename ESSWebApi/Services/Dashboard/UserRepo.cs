using ESSCommon.Core;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSWebApi.Dtos.Result;
using ESSWebApi.Dtos.Result.User;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Services.Dashboard
{
    public class UserRepo : IUser
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResult> GetDashboardData(int userId)
        {
            var user = await _context.Users
                 .Where(x => x.Id == userId)
                 .Include(x => x.Schedule)
                 .Select(x => new AppUser
                 {
                     Id = x.Id,
                     Schedule = new ScheduleModel
                     {
                         Sunday = x.Schedule.Sunday,
                         Monday = x.Schedule.Monday,
                         Tuesday = x.Schedule.Tuesday,
                         Wednesday = x.Schedule.Wednesday,
                         Thursday = x.Schedule.Tuesday,
                         Friday = x.Schedule.Friday,
                         Saturday = x.Schedule.Saturday,
                         StartTime = x.Schedule.StartTime,
                         EndTime = x.Schedule.EndTime
                     },
                     AvailableLeaveCount = x.AvailableLeaveCount
                 })
                 .FirstOrDefaultAsync();

            var now = DateTime.Now;

            if (user == null)
            {
                return new BaseResult
                {
                    Message = "Unable to load data"
                };
            }

            var attendances = await _context.Attendances
                .Where(x => x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year
                && x.EmployeeId == user.Id)
                .Select(x => new AttendanceModel
                {
                    CheckInTime = x.CheckInTime,
                    CheckOutTime = x.CheckOutTime,
                    Trackings = x.Trackings,
                    CreatedAt = x.CreatedAt,
                }).ToListAsync();

            int weeklyOffDays = 0;
            int absentDays = 0;
            int presentDays = 0;
            int halfDays = 0;
            int inLateDays = 0;
            int outLateDays = 0;
            double travelledDistance = 0.0;

            var dates = SharedHelper.GetDates(now.Year, now.Month);

            foreach (var date in dates)
            {
                if (date.Date > DateTime.Now.Date.AddDays(1)) break;

                TimeSpan shiftTimeHalf = (user.Schedule.EndTime - user.Schedule.StartTime) / 2;
                var attendance = attendances.FirstOrDefault(x => x.CreatedAt.Date == date.Date);

                if (attendance == null)
                {
                    if (!SharedHelper.IsWeekOff(user.Schedule, date))
                    {
                        absentDays++;
                    }
                    else
                    {
                        weeklyOffDays++;
                    }
                }
                else
                {
                    presentDays++;

                    if (attendance.Trackings.Count() > 1)
                    {
                        var filteredTrackings = attendance.Trackings
                            .OrderByDescending(x => x.CreatedAt)
                            .Where(x => x.Latitude != 0 && x.Longitude != 0)
                            .ToList();

                        if (filteredTrackings.Count() > 1)
                        {
                            TrackingModel previous = new TrackingModel();

                            for (int i = 0; i < filteredTrackings.Count(); i++)
                            {
                                var tracking = filteredTrackings[i];
                                if (i == 0)
                                {
                                    previous = tracking;
                                    continue;
                                }

                                travelledDistance += SharedHelper.DistanceBetweenPlaces(Convert.ToDouble(previous.Latitude), Convert.ToDouble(previous.Longitude), Convert.ToDouble(tracking.Latitude), Convert.ToDouble(tracking.Longitude));
                                if (double.Equals(double.NaN, travelledDistance))
                                    travelledDistance = 0;

                                //if (i > filteredTrackings.Count()) break;

                                previous = tracking;
                            }
                        }
                    }




                    if (attendance.CheckInTime.TimeOfDay < user.Schedule.StartTime.TimeOfDay)
                    {
                        inLateDays++;
                    }

                    if (!attendance.CheckOutTime.HasValue)
                    {
                        var lastData = await _context.Trackings
                            .Where(x => x.AttendanceId == attendance.Id)
                            .OrderByDescending(x => x.CreatedAt)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

                        if (lastData != null)
                        {
                            var data = lastData.CreatedAt - attendance.CheckInTime;
                        }
                    }
                    else
                    {
                        var data = attendance.CheckOutTime.Value - attendance.CheckInTime;


                        if (attendance.CheckOutTime.Value.TimeOfDay <= shiftTimeHalf)
                        {
                            halfDays++;
                        }
                        else if (attendance.CheckOutTime.Value.TimeOfDay > user.Schedule.EndTime.TimeOfDay)
                        {
                            outLateDays++;
                        }
                    }
                }
            }



            List<bool> weeks = new List<bool>
            {
                user.Schedule.Sunday,
                user.Schedule.Monday,
                user.Schedule.Tuesday,
                user.Schedule.Wednesday,
                user.Schedule.Thursday,
                user.Schedule.Friday,
                user.Schedule.Saturday
            };

            var expenses = await _context.ExpenseRequests
                .Where(x => x.UserId == userId)
                .Where(x => x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year)
                .Select(x => new ExpenseRequestModel
                {
                    Status = x.Status,
                    Amount = x.Amount,
                    ApprovedAmount = x.ApprovedAmount,
                })
                .AsNoTracking()
                .ToListAsync();


            GetDashboardResult result = new GetDashboardResult
            {
                ScheduleTime = user.Schedule.StartTime.ToString("hh:mm tt ") + "-" + user.Schedule.EndTime.ToString("hh:mm tt"),
                ScheduleWeeks = weeks,
                WeekOffDays = weeklyOffDays,
                PresentDays = presentDays,
                AbsentDays = absentDays,
                HalfDays = halfDays,
                AvailableLeaveCount = user.AvailableLeaveCount,
                //Travelled = Math.Round(travelledDistance, 2),
                Travelled = Math.Round(travelledDistance, 0),
                OnLeaveDays = 0,
                Approved = expenses.Where(x => x.Status == ExpenseStatusEnum.Approved && x.ApprovedAmount != null).Sum(x => x.ApprovedAmount.Value),
                Pending = expenses.Where(x => x.Status == ExpenseStatusEnum.Pending).Sum(x => x.Amount),
                Rejected = expenses.Where(x => x.Status == ExpenseStatusEnum.Rejected).Sum(x => x.Amount)
            };

            return new BaseResult
            {
                IsSuccess = true,
                Data = result,
            };

        }
    }
}
