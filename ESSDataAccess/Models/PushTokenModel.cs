using ESSDataAccess.Identity;
using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models
{
    public class PushTokenModel : BaseModel
    {
        public string Token { get; set; }

        public PushTokenType Type { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public AppUser? User { get; set; }

    }

    public enum PushTokenType
    {
        Web,
        IOS,
        Android,
        Other
    }
}
