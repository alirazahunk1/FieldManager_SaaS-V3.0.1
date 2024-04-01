using ESSCommon.Core;
using ESSDataAccess.DbContext;
using ESSDataAccess.Models;
using ESSWebApi.Dtos.Request.Visit;
using ESSWebApi.Dtos.Result;
using ESSWebApi.Dtos.Result.Visits;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Services.Visit
{
    public class VisitService : IVisit
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public VisitService(AppDbContext context,
            IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


        public async Task<BaseResult> GetHistory(int userId, DateTime date)
        {
            var visits = await _context.Visits
                .Where(x => x.Attendance.EmployeeId == userId && x.CreatedAt.Date == date.Date)
                .Include(x => x.Client)
                .Select(x => new VisitHistoryDto
                {
                    ClientAddress = x.Address,
                    ClientName = x.Client.Name,
                    VisitImage = x.ImgUrl,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    VisitRemarks = x.Remarks,
                    VisitDateTime = x.CreatedAt.ToString(SharedConstants.DateTimeFormatHuman),
                    ClientContactPerson = x.Client.ContactPerson,
                    ClientEmail = x.Client.Email,
                    ClientPhoneNumber = x.Client.PhoneNumber
                }).AsNoTracking()
                .ToListAsync();

            return new BaseResult
            {
                IsSuccess = true,
                Data = visits
            };
        }

        public async Task<BaseResult> GetVisitsCount(int userId)
        {
            var todaysVisits = await _context.Visits
                 .Where(x => x.Attendance.EmployeeId == userId && x.CreatedAt.Date == DateTime.Now.Date)
                 .CountAsync();

            var totalVisits = await _context.Visits
                .Where(x => x.Attendance.EmployeeId == userId)
                .CountAsync();

            return new BaseResult
            {
                IsSuccess = true,
                Data = new VisitCountResult
                {
                    TodaysVisits = todaysVisits,
                    TotalVisits = totalVisits
                }
            };
        }


        public async Task<BaseResult> Create(int userId, CreateVisitRequest request, IFormFile file)
        {
            var attendance = await _context.Attendances.Where(x => x.EmployeeId == userId && x.CreatedAt.Date == DateTime.Now.Date)
                .Include(x => x.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (attendance == null)
                return new BaseResult
                {
                    Message = "Attendance not created"
                };

            if (request.ClientId != null && !await _context.Clients.AnyAsync(x => x.Id == request.ClientId.Value))
                return new BaseResult
                {
                    Message = "Invalid client Id"
                };

            var filePath = await SaveVisitImage(attendance.Employee.UserName, file);

            VisitModel visit = new VisitModel
            {
                AttendanceId = attendance.Id,
                Address = request.Address,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                ImgUrl = filePath,
                ClientId = request.ClientId,
                Latitude = request.Latitude ?? 0,
                Longitude = request.Longitude ?? 0,
                Remarks = request.Remarks,
            };

            await _context.AddAsync(visit);
            await _context.SaveChangesAsync();

            return new BaseResult
            {
                IsSuccess = true
            };

        }


        private async Task<string> SaveVisitImage(string userName, IFormFile file)
        {
            string folderName = $"Visits/{userName}/";
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
