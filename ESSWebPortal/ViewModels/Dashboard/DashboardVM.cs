namespace ESSWebPortal.ViewModels.Dashboard
{
    public class DashboardVM
    {
        public int LeaveRequestCount { get; set; } = 0;

        public int SignBoardRequestCount { get; set; } = 0;

        public int PhotoUploadsCount { get; set; } = 0;

        public int ExpenseRequestsCount { get; set; } = 0;

        public int EmployeesCount { get; set; } = 0;

        public int ClientsCount { get; set; }

        public string User { get; set; }

        public int OnlineCount { get; set; } = 0;

        public int InActiveCount { get; set; } = 0;

        public int OfflineCount { get; set; } = 0;

        public int NotWorkingCount { get; set; } = 0;

        public List<EmployeeItemVM> EmployeeItems { get; set; }
    }

    public partial class EmployeeItemVM
    {
        public string Name { get; set; }

        public string AttendanceInAt { get; set; }

        public string AttendanceOutAt { get; set; }

        public string Address { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string LastUpdate { get; set; }
    }
}
