using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSWebApi.Dtos.Result;
using ESSWebApi.Manager.ManagerDtos.Requests.Expense;
using ESSWebApi.Manager.ManagerDtos.Requests.Leave;
using ESSWebApi.Manager.ManagerDtos.Result.Dashboard;
using ESSWebApi.Manager.ManagerDtos.Result.LeaveRequest;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Services.Manager
{
    public class ManagerRepo : IManager
    {
        private readonly AppDbContext _context;

        public ManagerRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResult> GetDashboardData(int userId)
        {
            var now = DateTime.Now;

            var totalUsersCount = await _context.Users
                .Where(x => x.ParentId == userId && x.Status == UserStatus.Active)
                .CountAsync();

            var users = await _context.Users
                .Where(x => x.ParentId == userId && x.Status == UserStatus.Active)
                .Include(x => x.UserDevice)
                .Include(x => x.Attendances)
                .Where(x => x.Attendances.Any(y => y.CreatedAt.Date == now.Date))
                .Select(x => new User
                {
                    EmployeeId = x.Id,
                    Name = x.GetFullName(),
                    CheckedInTime = x.Attendances.FirstOrDefault().CheckInTime.ToString("dd/MM/yyy hh:mm tt"),
                    CheckOutTime = x.Attendances.FirstOrDefault().CheckOutTime == null ? string.Empty : x.Attendances.FirstOrDefault().CheckOutTime.Value.ToString("dd/MM/yyy hh:mm tt"),
                    Device = new UserDevice
                    {
                        Address = x.UserDevice.Address,
                        DeviceType = x.UserDevice.DeviceType,
                        BatteryPercentage = x.UserDevice.BatteryPercentage,
                        Brand = x.UserDevice.Brand,
                        IsGPSOn = x.UserDevice.IsGPSOn,
                        IsWifiOn = x.UserDevice.IsWifiOn,
                        Latitude = x.UserDevice.Latitude,
                        Longitude = x.UserDevice.Longitude,
                        Model = x.UserDevice.Model,
                        LastUpdatedTime = x.UserDevice.UpdatedAt.ToString("dd/MM/yyy hh:mm tt"),
                    }
                })
                .AsNoTracking()
                .ToListAsync();

            var onLeaveUsersCount = await _context.LeaveRequests
                .AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.FromDate.Date <= DateTime.Now.Date && x.ToDate.Date >= DateTime.Now.Date && x.User.ParentId == userId && x.Status == LeaveRequestStatus.Approved)
                .CountAsync();



            GetManagerDashboardResult result = new GetManagerDashboardResult
            {
                Users = users,
                TotalUsersCount = totalUsersCount,
                CheckedInUsersCount = users.Count,
                CheckInPendingUsersCount = totalUsersCount - users.Count - onLeaveUsersCount,
                AbsentUsersCount = onLeaveUsersCount
            };

            return new BaseResult
            {
                IsSuccess = true,
                Data = result
            };
        }

        public async Task<BaseResult> GetAllLeaveRequests(int userId)
        {
            var leaveRequests = await _context.LeaveRequests
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.LeaveType)
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.User.ParentId == userId)
                .Select(x => new LeaveRequestResult
                {
                    Id = x.Id,
                    EmployeeId = x.UserId,
                    Type = x.LeaveType.Name,
                    EmployeeName = x.User.UserName,
                    FromDate = x.FromDate.ToString("dd/MM/yyyy"),
                    ToDate = x.ToDate.ToString("dd/MM/yyyy"),
                    Status = x.Status.ToString(),
                })
                .ToListAsync();

            return new BaseResult
            {
                IsSuccess = true,
                Data = leaveRequests
            };
        }

        public async Task<BaseResult> ChangeLeaveRequestStatus(ChangeLeaveStatus request, int userId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(request.Id);

            if (leaveRequest == null) return new BaseResult { Message = "Request not found" };

            var now = DateTime.Now;


            if (request.Status.ToLower() == LeaveRequestStatus.Approved.ToString().ToLower())
            {
                leaveRequest.Status = LeaveRequestStatus.Approved;
                leaveRequest.ApproverRemarks = request.Remarks != String.Empty ? request.Remarks : null;
                leaveRequest.ApprovedOn = now;
                leaveRequest.ApprovedBy = userId;
                leaveRequest.UpdatedAt = now;
                leaveRequest.UpdatedBy = userId;
            }
            else
            {
                leaveRequest.Status = LeaveRequestStatus.Rejected;
                leaveRequest.ApproverRemarks = request.Remarks != String.Empty ? request.Remarks : null;
                leaveRequest.ApprovedOn = now;
                leaveRequest.ApprovedBy = userId;
                leaveRequest.UpdatedAt = now;
                leaveRequest.UpdatedBy = userId;
            }

            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();

            return new BaseResult
            {
                IsSuccess = true,
                Message = $"Leaverequest was {request.Status}."
            };
        }

        public async Task<BaseResult> GetAllExpenseRequests(int userId)
        {
            var expenceRequests = await _context.ExpenseRequests
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.ExpenseType)
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.User.ParentId == userId)
                .Select(x => new ExpenseRequestResult
                {
                    Id = x.Id,
                    EmployeeId = x.UserId,
                    Type = x.ExpenseType.Name,
                    EmployeeName = x.User.UserName,
                    Date = x.ForDate.ToString("dd/MM/yyyy"),
                    Amount = x.Amount,
                    ApprovedAmount = x.ApprovedAmount,
                    Status = x.Status.ToString(),
                })
                .ToListAsync();

            return new BaseResult
            {
                IsSuccess = true,
                Data = expenceRequests
            };
        }

        public async Task<BaseResult> ChangeExpenseRequestStatus(ChangeExpenseStatus request, int userId)
        {
            var expenseRequest = await _context.ExpenseRequests.FindAsync(request.Id);

            if (expenseRequest == null) return new BaseResult { Message = "Request not found" };

            var now = DateTime.Now;


            if (request.Status.ToLower() == ExpenseStatusEnum.Approved.ToString().ToLower())
            {
                expenseRequest.Status = ExpenseStatusEnum.Approved;
                expenseRequest.ApproverRemarks = request.Remarks != String.Empty ? request.Remarks : null;
                expenseRequest.ApprovedAmount = request.ApprovedAmount;
                expenseRequest.ApprovedOn = now;
                expenseRequest.Approvedby = userId;
                expenseRequest.UpdatedAt = now;
                expenseRequest.UpdatedBy = userId;
            }
            else
            {
                expenseRequest.Status = ExpenseStatusEnum.Rejected;
                expenseRequest.ApproverRemarks = request.Remarks != String.Empty ? request.Remarks : null;
                expenseRequest.ApprovedOn = now;
                expenseRequest.Approvedby = userId;
                expenseRequest.UpdatedAt = now;
                expenseRequest.UpdatedBy = userId;
            }

            _context.ExpenseRequests.Update(expenseRequest);
            await _context.SaveChangesAsync();

            return new BaseResult
            {
                IsSuccess = true,
                Message = $"Expense request was {request.Status}."
            };
        }

        public async Task<BaseResult> GetEmployeeStatus(int userId)
        {
            var users = await _context.Users
               .Where(x => x.ParentId == userId && x.Status == UserStatus.Active)
               .Include(x => x.UserDevice)
               .Include(x => x.Attendances)
               .Where(x => x.Attendances.Any(y => y.CreatedAt.Date == DateTime.Now.Date))
               .Select(x => new User
               {
                   EmployeeId = x.Id,
                   Name = x.GetFullName(),
                   CheckedInTime = x.Attendances.FirstOrDefault().CheckInTime.ToString("dd/MM/yyy hh:mm tt"),
                   CheckOutTime = x.Attendances.FirstOrDefault().CheckOutTime == null ? string.Empty : x.Attendances.FirstOrDefault().CheckOutTime.Value.ToString("dd/MM/yyy hh:mm tt"),
                   Device = new UserDevice
                   {
                       Address = x.UserDevice.Address,
                       DeviceType = x.UserDevice.DeviceType,
                       BatteryPercentage = x.UserDevice.BatteryPercentage,
                       Brand = x.UserDevice.Brand,
                       IsGPSOn = x.UserDevice.IsGPSOn,
                       IsWifiOn = x.UserDevice.IsWifiOn,
                       Latitude = x.UserDevice.Latitude,
                       Longitude = x.UserDevice.Longitude,
                       Model = x.UserDevice.Model,
                       LastUpdatedTime = x.UserDevice.UpdatedAt.ToString("dd/MM/yyy hh:mm tt"),
                   }
               })
               .AsNoTracking()
               .ToListAsync();

            return new BaseResult
            {
                IsSuccess = true,
                Data = users
            };
        }
    }
}
