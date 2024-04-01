using ESSDataAccess.Models.Chat;

namespace ESSWebPortal.ViewModels.Chat
{
    public class ChatDataVM
    {
        public bool IsYou { get; set; } = false;

        public string From { get; set; }

        public string Message { get; set; }

        public string Time { get; set; }

        public ChatTypeEnum Type { get; set; }

        public string ImageUrl { get; set; }
    }
}
