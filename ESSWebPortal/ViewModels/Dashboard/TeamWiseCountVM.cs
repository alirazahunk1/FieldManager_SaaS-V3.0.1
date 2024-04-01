namespace ESSWebPortal.ViewModels.Dashboard
{
    public class TeamWiseCountVM
    {
        public string Name { get; set; }

        public int OnlineCount { get; set; } = 0;

        public int InActiveCount { get; set; } = 0;

        public int OfflineCount { get; set; } = 0;

        public int NotWorkingCount { get; set; } = 0;
    }
}
