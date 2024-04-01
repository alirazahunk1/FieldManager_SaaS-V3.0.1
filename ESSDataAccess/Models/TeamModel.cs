using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using ESSDataAccess.Models.Chat;
using System.ComponentModel.DataAnnotations;

namespace ESSDataAccess.Models
{
    public class TeamModel : BaseModel
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public TeamStatus Status { get; set; }

        public bool IsChatEnabled { get; set; } = true;

        public List<AppUser>? Users { get; set; }

        public List<ChatModel>? Chats { get; set; }
    }
}
