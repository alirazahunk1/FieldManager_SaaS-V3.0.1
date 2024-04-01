using ESSDataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.Core.ViewModel.SuperAdmin
{
    public class OrderVM
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string OrderId { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        public string? Tenant { get; set; }

        public string? Plan { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal Amount { get; set; }

        public SAOrderStatus status { get; set; }
    }
}
