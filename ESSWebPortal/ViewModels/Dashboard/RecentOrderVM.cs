namespace ESSWebPortal.ViewModels.Dashboard
{
    public class RecentOrderVM
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public decimal Amount { get; set; }

        public int ItemsCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; }
    }
}
