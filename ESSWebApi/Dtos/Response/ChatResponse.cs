namespace ESSWebApi.Dtos.Response
{
    public class ChatResponse
    {
        public string TeamName { get; set; }

        public int TeamId { get; set; }

        public bool IsChatEnabled { get; set; }

        public List<ChatItem> ChatItems { get; set; }
    }

    public partial class ChatItem
    {

        public int Id { get; set; }

        public string? From { get; set; }

        public string Message { get; set; }

        public string CreatedAt { get; set; }

        public string ChatType { get; set; }

        public string? FileUrl { get; set; }
    }
}
