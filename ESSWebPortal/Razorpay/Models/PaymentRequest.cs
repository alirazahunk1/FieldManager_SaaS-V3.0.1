using System.ComponentModel.DataAnnotations;

namespace ESSWebPortal.Razorpay.Models
{
    public class PaymentRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        public string OrderId { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
