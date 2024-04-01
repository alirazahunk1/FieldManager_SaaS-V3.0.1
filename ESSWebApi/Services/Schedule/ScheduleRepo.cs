using AutoMapper;
using ESSDataAccess.DbContext;
using ESSDataAccess.Dtos.API_Dtos.Schedule;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Services.Schedule
{
    public class ScheduleRepo : IScheduleManager
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public ScheduleRepo(AppDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper =   mapper;
        }

        public async Task<ScheduleModel> AddSeederSchedule(ScheduleModel data)
        {
            await _db.AddAsync(data);
            await _db.SaveChangesAsync();
            return data;
        }

        public async Task<ScheduleDto> GetScheduleById(int? scheduleId)
        {
            var scheduleData =  _db.Schedules
                .Where(x => x.Id == scheduleId)
                .AsNoTracking()
                .FirstOrDefault();

            ScheduleDto result = _mapper.Map<ScheduleDto>(scheduleData);

            return result;
        }

        public async Task<ScheduleDto> GetUserSchedule(int UserId)
        {

            var userData = await _db.Users
                .Where(x => x.Id == UserId)
                .Include(x => x.Schedule)
                .Select(x => new AppUser
                {
                    Id = x.Id,
                    Schedule = new ScheduleModel
                    {
                        Id = x.Schedule.Id,
                        Title = x.Schedule.Title,
                        Status = x.Schedule.Status,
                    }
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            ScheduleDto result = _mapper.Map<ScheduleDto>(userData.Schedule);

            return result;
        }
    }
}
