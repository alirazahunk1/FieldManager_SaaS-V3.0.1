using AutoMapper;
using ESSDataAccess.DbContext;
using ESSDataAccess.Dtos.API_Dtos.Notification;
using Microsoft.EntityFrameworkCore;

namespace ESSWebApi.Services.Notification
{
    public class NotificationRepo : INotification
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        public NotificationRepo(AppDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        public async Task<List<NotificationDto>> GetNotifications()
        {
            var data = await _db.Notifications.AsNoTracking().ToListAsync();
            List<NotificationDto> result = _mapper.Map<List<NotificationDto>>(data);
            return result;
        }



        public async Task<bool> SetAsReaded(int? Id)
        {
            var data = await _db.Notifications.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
            if (data == null)
                return false;

            data.IsViewed = true;
            data.UpdatedAt = DateTime.Now;
            _db.Update(data);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
