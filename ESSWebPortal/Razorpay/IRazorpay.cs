using ESSWebPortal.Razorpay.Models;

namespace ESSWebPortal.Razorpay
{
    public interface IRazorpay
    {
        Task<MerchantOrder> ProcessMerchantOrder(PaymentRequest payRequest);
        Task<string> CompleteOrderProcess(IHttpContextAccessor _httpContextAccessor);
    }
}
