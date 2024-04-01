using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Notice
{
    public class TeamNoticeModel : BaseModel
    {
        [ForeignKey(nameof(Notice))]
        public int NoticeId { get; set; }

        public NoticeModel? Notice { get; set; }

        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }

        public TeamModel? Team { get; set; }
    }
}
