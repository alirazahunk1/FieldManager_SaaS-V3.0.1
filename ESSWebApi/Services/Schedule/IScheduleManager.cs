using ESSDataAccess.Dtos.API_Dtos.Schedule;
using ESSDataAccess.Models;

namespace ESSWebApi.Services.Schedule
{
    public interface IScheduleManager
    {
        Task<ScheduleModel> AddSeederSchedule(ScheduleModel data);
        Task<ScheduleDto> GetScheduleById(int? scheduleId);
        Task<ScheduleDto> GetUserSchedule(int UserId);
       
    }
}
