
namespace ESSWebApi.Dtos.Request.Expense
{
    public class CreateExpenseRequest
    {
        public string? Date { get; set; }

        public double? Amount { get; set; }

        public int? TypeId { get; set; }

        public string Comments { get; set; }
    }
}
