namespace ESSWebPortal.ViewModels.Dashboard
{
    public class CardViewVM
    {
        public List<User> Users { get; set; }

        public List<Team> Teams { get; set; }
    }

    public partial class Team
    {
        public int TeamId { get; set; }

        public string Name { get; set; }
    }

    public partial class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsGpsOn { get; set; }

        public int SignalLevel { get; set; }

        public bool IsWifiOn { get; set; }

        public int BatteryLevel { get; set; }

        public string UpdatedAt { get; set; }

        public string AttendanceInAt { get; set; }

        public string AttendanceOutAt { get; set; }

        public decimal Latitude { get; set; } = 0;

        public decimal Longitude { get; set; } = 0;

        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public bool IsOnline { get; set; }
    }
}
