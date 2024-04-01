namespace ESSWebApi.Services.Manager
{
    internal class ExpenseRequestResult
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Type { get; set; }
        public string EmployeeName { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public string Status { get; set; }
    }
}