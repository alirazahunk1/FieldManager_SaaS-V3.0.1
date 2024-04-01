using ESSDataAccess.Models;

namespace CZ.WebCore.SuperAdmin.ViewModels.Dashboard
{
    public class SARecentOrderVM
    {
        public string OrderId { get; set; }

        public string PurchaseDate { get; set; }

        public string Plan { get; set; }

        public decimal Amount { get; set; }

        public SAOrderStatus Status { get; set; }
    }
}
