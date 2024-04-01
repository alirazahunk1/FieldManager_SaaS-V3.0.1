using ESSDataAccess.Enum;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models.Notice
{
    public class NoticeModel : BaseModel
    {
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(3000)]
        public string Description { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public CommonStatus Status { get; set; } = CommonStatus.Active;

        public NoticeForEnum NoticeFor { get; set; }

        public List<UserNoticeModel>? UserNotices { get; set; }

        public List<TeamNoticeModel>? TeamNotices { get; set; }
    }

    public enum NoticeForEnum
    {
        All,
        Employees,
        Teams,
    }
}
