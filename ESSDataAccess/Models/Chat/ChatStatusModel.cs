using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Chat
{
    public class ChatStatusModel : BaseModel
    {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public AppUser User { get; set; }

        public bool IsOnline { get; set; } = false;

        public bool IsTyping { get; set; } = false;

        public string? StatusMessage { get; set; } 

        public ChatStatus Status { get; set; } = ChatStatus.Away;
    }
}
