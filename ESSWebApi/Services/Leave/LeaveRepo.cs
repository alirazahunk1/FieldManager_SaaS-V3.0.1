using ESSCommon.Core.Services.Notification;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSWebApi.Dtos.Request.Leave;
using ESSWebApi.Dtos.Result;
using ESSWebApi.Dtos.Result.Leave;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ESSWebApi.Services.Leave
{
    public class LeaveRepo : ILeave
    {
        private readonly AppDbContext _context;
        private readonly INotification _notification;

        public LeaveRepo(AppDbContext context,
            INotification notification)
        {
            _context = context;
            _notification = notification;
        }

        public async Task<BaseResult> CreateLeaveRequest(int userId, CreateLeaveRequest request)
        {

            try
            {
                var leaveRequest = new LeaveRequestModel
                {
                    UserId = userId,
                    FromDate = DateTime.ParseExact(request.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ToDate = DateTime.ParseExact(request.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    LeaveTypeId = request.LeaveType,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Comments = request.Comments,
                    Status = LeaveRequestStatus.Pending,
                };


                await _context.AddAsync(leaveRequest);

                await _context.SaveChangesAsync();

                await _notification.PostLeaveRequest(userId);

                return new BaseResult
                {
                    IsSuccess = true
                };

            }
            catch (Exception ex)
            {
                return new BaseResult
                {
                    Message = ex.Message + " " + ex.StackTrace
                };
            }
        }

        public async Task DeleteRequest(int requestId)
        {
            var request = await _context.LeaveRequests!
                .FirstOrDefaultAsync(x => x.Id == requestId);
            _context.Remove(request);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> GetLastRequestId(int userId)
        {
            return await _context.LeaveRequests!
                 .Where(x => x.UserId == userId)
                 .OrderByDescending(x => x.CreatedAt)
                 .Select(x => x.Id)
                 .FirstOrDefaultAsync();
        }

        public async Task<BaseResult> GetLeaveRequests(int userId)
        {

            var users = await _context.Users
                .Where(x => x.Status == UserStatus.Active)
                .Select(x => new AppUser { Id = x.Id, FirstName = x.FirstName, LastName = x.LastName })
                .ToListAsync();

            try
            {
                var requests = await _context.LeaveRequests!
                .Where(x => x.UserId == userId)
                .Include(x => x.LeaveType)
                .Select(x => new LeaveRequestDto
                {
                    Id = x.Id,
                    FromDate = x.FromDate.ToString("dd/MM/yyyy"),
                    ToDate = x.ToDate.ToString("dd/MM/yyyy"),
                    LeaveType = x.LeaveType.Name,
                    Comments = x.Comments,
                    Status = x.Status.ToString(),
                    Image = x.Document,
                    CreatedOn = x.CreatedAt.ToString("dd/MM/yyy HH:mm"),
                    ApprovedBy = x.ApprovedBy.ToString(),
                    ApprovedOn = x.ApprovedOn != null ? x.ApprovedOn.Value.ToString("dd/MM/yyyy") : "-",
                }).ToListAsync();

                return new BaseResult
                {
                    IsSuccess = true,
                    Data = requests
                };
            }
            catch (Exception ex)
            {
                return new BaseResult
                {
                    Message = ex.Message + " " + ex.StackTrace
                };
            }
        }

        public async Task<List<LeaveTypeDto>> GetLeaveTypes()
        {
            return await _context.LeaveTypes
                .Select(x => new LeaveTypeDto
                { Id = x.Id, Name = x.Name, IsImgRequired = x.IsImgRequired })
                .ToListAsync();
        }

        public async Task UpdateDocument(int requestId, string documentUrl)
        {
            var request = await _context.LeaveRequests.FindAsync(requestId);
            if (request != null)
            {
                request.Document = documentUrl;
                request.UpdatedAt = DateTime.Now;
                _context.Update(request);
                await _context.SaveChangesAsync();
            }
        }
    }
}
