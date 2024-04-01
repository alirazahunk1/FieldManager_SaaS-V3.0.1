namespace ESSWebApi.Manager.ManagerDtos.Requests.Expense
{
    public class ChangeExpenseStatus
    {
        public int Id { get; set; }

        public string? Remarks { get; set; }

        public decimal? ApprovedAmount { get; set; }

        public string? Status { get; set; }
    }
}
