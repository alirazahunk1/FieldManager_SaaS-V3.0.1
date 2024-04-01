

namespace ESSWebApi.Dtos.Result.Expense
{
    public class ExpenseRequestDto
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string Type { get; set; }

        public decimal ActualAmount { get; set; }

        public decimal? ApprovedAmount { get; set; }

        public string Status { get; set; }

        public string CreatedAt { get; set; }

        public string ApprovedBy { get; set; }
    }
}
