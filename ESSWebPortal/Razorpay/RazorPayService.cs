using ESSWebPortal.Core.SuperAdmin;
using ESSWebPortal.Razorpay.Models;
using Razorpay.Api;

namespace ESSWebPortal.Razorpay
{
    public class RazorPayService : IRazorpay
    {
        private readonly ISASettings _sASettings;

        public RazorPayService(ISASettings sASettings)
        {
            _sASettings = sASettings;
        }
        public async Task<MerchantOrder> ProcessMerchantOrder(PaymentRequest payRequest)
        {

            try
            {
                var razorPaySettings = await _sASettings.GetRazorPaySettings();

                var currency = await _sASettings.GetCurrency();

                // Generate random receipt number for order
                Random randomObj = new Random();
                string transactionId = randomObj.Next(10000000, 100000000).ToString();
                RazorpayClient client = new RazorpayClient(razorPaySettings.RazorPayKeyId, razorPaySettings.RazorPayKeySecret);
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", payRequest.Amount * 100);
                options.Add("receipt", payRequest.OrderId);
                options.Add("currency", currency);
                options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
                                                     //options.Add("Notes", "Test Payment of Merchant");
                Order orderResponse = client.Order.Create(options);
                string orderId = orderResponse["id"].ToString();
                MerchantOrder order = new MerchantOrder
                {
                    OrderId = orderResponse.Attributes["id"],
                    RazorpayKey = razorPaySettings.RazorPayKeyId,
                    Amount = payRequest.Amount * 100,
                    Currency = currency,
                    Name = payRequest.Name,
                    Email = payRequest.Email,
                    PhoneNumber = payRequest.PhoneNumber,
                    Address = "",
                    Description = "Order by Merchant"
                };
                return order;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> CompleteOrderProcess(IHttpContextAccessor _httpContextAccessor)
        {
            try
            {

                var razorPaySettings = await _sASettings.GetRazorPaySettings();

                string paymentId = _httpContextAccessor.HttpContext.Request.Form["rzp_paymentid"];
                // This is orderId
                string orderId = _httpContextAccessor.HttpContext.Request.Form["rzp_orderid"];
                RazorpayClient client = new RazorpayClient(razorPaySettings.RazorPayKeyId, razorPaySettings.RazorPayKeySecret);
                Payment payment = client.Payment.Fetch(paymentId);
                // This code is for capture the payment 
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", payment.Attributes["amount"]);
                Payment paymentCaptured = payment.Capture(options);
                string amt = paymentCaptured.Attributes["amount"];
                return paymentCaptured.Attributes["status"];
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
