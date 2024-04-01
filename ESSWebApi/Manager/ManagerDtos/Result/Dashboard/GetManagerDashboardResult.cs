namespace ESSWebApi.Manager.ManagerDtos.Result.Dashboard
{

    public class GetManagerDashboardResult
    {
        public List<User>? Users { get; set; }

        public int CheckedInUsersCount { get; set; } = 0;

        public int AbsentUsersCount { get; set; } = 0;

        public int CheckInPendingUsersCount { get; set; } = 0;

        public int TotalUsersCount { get; set; } = 0;
    }

    //Users
    public class User
    {
        public string Name { get; set; }

        public int EmployeeId { get; set; }

        public string? CheckedInTime { get; set; }

        public string? CheckOutTime { get; set; }

        public UserDevice Device { get; set; }

    }

    public class UserDevice
    {
        public string DeviceType { get; set; }

        public string? LastUpdatedTime { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int BatteryPercentage { get; set; } = 0;

        public bool IsGPSOn { get; set; } = false;

        public bool IsWifiOn { get; set; } = false;

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string? Address { get; set; } = string.Empty;
    }
}
