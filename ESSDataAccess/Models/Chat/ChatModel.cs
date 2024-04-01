using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Chat
{
    public class ChatModel : BaseModel
    {

        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }

        public TeamModel? Team { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public AppUser? User { get; set; }

        [MaxLength(500)]
        public string? Message { get; set; }

        public bool IsRead { get; set; } = false;

        [MaxLength(1000)]
        public string? ImageUrl { get; set; }

        public ChatTypeEnum Type { get; set; } = ChatTypeEnum.Text;

    }

    public enum ChatTypeEnum
    {
        Text,
        Image,
        File,
        Video,
        Audio,
        Location,
        Contact,
        Sticker,
        Gif,
        Poll,
        Reply,
        Forward,
    }
}
