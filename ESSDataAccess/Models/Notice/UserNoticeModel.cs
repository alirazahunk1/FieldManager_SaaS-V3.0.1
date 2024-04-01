using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Notice
{
    public class UserNoticeModel : BaseModel
    {
        [ForeignKey(nameof(Notice))]
        public int NoticeId { get; set; }

        public NoticeModel? Notice { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public AppUser? User { get; set; }
    }
}
