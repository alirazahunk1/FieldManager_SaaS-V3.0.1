using ESSDataAccess.DbContext;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSDataAccess.Tenant;
using ESSDataAccess.Tenant.Models;
using ESSWebPortal.Core.Plan;
using ESSWebPortal.Core.SuperAdmin;
using ESSWebPortal.Paypal;
using ESSWebPortal.Razorpay;
using ESSWebPortal.Razorpay.Models;
using ESSWebPortal.ViewModels.Plan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;
using PayPal.Api;

namespace ESSWebPortal.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private Payment payment;
        private readonly IPlan _plan;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISASettings _settings;
        private readonly AppDbContext _context;
        private readonly IToastNotification _toast;
        private readonly IRazorpay _razorpay;
        private readonly ITenant _tenant;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentController(IPlan plan,
            UserManager<AppUser> userManager,
            ISASettings settings,
            AppDbContext context,
            IToastNotification toast,
            IRazorpay razorpay,
            ITenant tenant,
            IHttpContextAccessor httpContextAccessor)
        {
            _plan = plan;
            _userManager = userManager;
            _settings = settings;
            _context = context;
            _toast = toast;
            _razorpay = razorpay;
            _tenant = tenant;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> MyPlan()
        {
            var user = await _userManager.GetUserAsync(User);
            var tenant = await _context.Tenants
                .Include(x => x.Plan)
                .Include(x => x.Subscriptions)
                .FirstOrDefaultAsync(x => x.Id == user.TenantId);

            var currentSubscription = tenant.Subscriptions
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            MyPlanVM vm = new MyPlanVM
            {
                CreatedAt = tenant.Plan.CreatedAt,
                UpdatedAt = tenant.Plan.UpdatedAt,
                Description = tenant.Plan.Description,
                ExpireDate = currentSubscription.EndDate,
                Id = tenant.Plan.Id,
                IsPromotional = tenant.Plan.IsPromotional,
                Name = tenant.Plan.Name,
                Price = tenant.Plan.Price,
                Status = tenant.Plan.Status,
                Type = tenant.Plan.Type,
                ModuleSettings = new ESSCommon.Core.SharedModels.Settings.ModuleSettingsDto
                {
                    IsBiometricVerificationModuleEnabled = tenant.Plan.IsBiometricVerificationModuleEnabled,
                    IsAiChatModuleEnabled = tenant.Plan.IsAiChatModuleEnabled,
                    IsBreakModuleEnabled = tenant.Plan.IsBreakModuleEnabled,
                    IsChatModuleEnabled = tenant.Plan.IsChatModuleEnabled,
                    IsClientVisitModuleEnabled = tenant.Plan.IsClientVisitModuleEnabled,
                    IsDataImportExportModuleEnabled = tenant.Plan.IsDataImportExportModuleEnabled,
                    IsDocumentModuleEnabled = tenant.Plan.IsDocumentModuleEnabled,
                    IsDynamicFormModuleEnabled = tenant.Plan.IsDynamicFormModuleEnabled,
                    IsDynamicQrCodeAttendanceEnabled = tenant.Plan.IsDynamicQrCodeAttendanceEnabled,
                    IsExpenseModuleEnabled = tenant.Plan.IsExpenseModuleEnabled,
                    IsGeofenceModuleEnabled = tenant.Plan.IsGeofenceModuleEnabled,
                    IsLeaveModuleEnabled = tenant.Plan.IsLeaveModuleEnabled,
                    IsIpBasedAttendanceModuleEnabled = tenant.Plan.IsIpBasedAttendanceModuleEnabled,
                    IsLoanModuleEnabled = tenant.Plan.IsLoanModuleEnabled,
                    IsNoticeModuleEnabled = tenant.Plan.IsNoticeModuleEnabled,
                    IsOfflineTrackingModuleEnabled = tenant.Plan.IsOfflineTrackingModuleEnabled,
                    IsPaymentCollectionModuleEnabled = tenant.Plan.IsPaymentCollectionModuleEnabled,
                    IsProductModuleEnabled = tenant.Plan.IsProductModuleEnabled,
                    IsQrCodeAttendanceModuleEnabled = tenant.Plan.IsQrCodeAttendanceModuleEnabled,
                    IsSiteModuleEnabled = tenant.Plan.IsSiteModuleEnabled,
                    IsTaskModuleEnabled = tenant.Plan.IsTaskModuleEnabled,
                    IsUidLoginModuleEnabled = tenant.Plan.IsUidLoginModuleEnabled,
                },
                SubscriptionStatus = tenant.SubscriptionStatus,
                TotalEmployeesCount = tenant.TotalEmployeesCount,
                AvailableEmployeesCount = tenant.AvailableEmployeesCount,
            };

            return View(vm);
        }

        public async Task<IActionResult> Index()
        {
            var plans = await _plan.GetAll();
            return View(plans);
        }


        public async Task<IActionResult> PayWithRazorPay(int? planId, int? employeeCount = null)
        {
            var orderModel = await CreateOrder(planId.Value, employeeCount.Value, PaymentGateway.Paypal);

            var user = await _userManager.GetUserAsync(User);

            PaymentRequest paymentRequest = new PaymentRequest
            {
                OrderId = orderModel.OrderId,
                Amount = orderModel.PerEmployeeAmount * employeeCount.Value,
                Name = user.GetFullName(),
                Address = "",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };

            var result = await _razorpay.ProcessMerchantOrder(paymentRequest);

            result.DbOrderId = orderModel.Id;

            return View("RazorPayPayment", result);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrderProcess()
        {
            string orderId = _httpContextAccessor.HttpContext.Request.Form["orderid"];
            if (string.IsNullOrEmpty(orderId)) return NotFound();

            string PaymentMessage = await _razorpay.CompleteOrderProcess(_httpContextAccessor);
            if (PaymentMessage == "captured")
            {
                await HandleSuccessResponse(Convert.ToInt32(orderId), PaymentMessage);

                _toast.AddSuccessToastMessage("Payment success");
                return RedirectToAction("AddInitialSettings", "Settings");
            }
            else
            {
                await HandleFailedResponse(Convert.ToInt32(orderId), PaymentMessage, false);
                _toast.AddErrorToastMessage("Payment failed");
                return View("PaymentFailed");
            }
        }


        public async Task<ActionResult> PaymentWithPaypal(string? Cancel = null, string blogId = "", string PayerID = "", string guid = "", int? planId = null, int? employeeCount = 1)
        {

            var orderModel = new OrderModel();

            string planName = string.Empty;
            if (string.IsNullOrEmpty(PayerID))
            {
                orderModel = await CreateOrder(planId.Value, employeeCount.Value, PaymentGateway.Paypal);

                var plan = await GetPlan(planId.Value);

                if (orderModel == null || plan == null) return RedirectToAction(nameof(Index));

                planName = plan.Name;
            }
            else
            {
                orderModel = await _context.Orders
                    .Where(x => x.TenantId == _tenant.TenantId)
                    .Where(x => x.Status == SAOrderStatus.Created)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync();



                if (orderModel == null)
                {
                    _toast.AddInfoToastMessage("Unable to process payment");
                    return RedirectToAction(nameof(Index));
                }
            }
            var paypalSettings = await _settings.GetPaypalSettings();
            //getting the apiContext  
            var ClientID = paypalSettings.PaypalClientId;
            var ClientSecret = paypalSettings.PaypalClientSecret;
            var mode = paypalSettings.PaypalMode == PaypalModeEnum.Sandbox ? "sandbox" : "live";
            APIContext apiContext = PaypalConfiguration.GetAPIContext(ClientID, ClientSecret, mode);
            // apiContext.AccessToken="Bearer access_token$production$j27yms5fthzx9vzm$c123e8e154c510d70ad20e396dd28287";
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = PayerID;
                if (string.IsNullOrEmpty(payerId))
                {

                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/Payment/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  

                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = await this.CreatePayment(apiContext, baseURI + "guid=" + guid, blogId, planName, orderModel.Description, (orderModel.PerEmployeeAmount * employeeCount.Value).ToString());
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string? paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    _httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  

                    var paymentId = _httpContextAccessor.HttpContext.Session.GetString("payment");
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        await HandleFailedResponse(orderModel.Id, JsonConvert.SerializeObject(executedPayment));
                        _toast.AddErrorToastMessage("Payment failed");
                        return View("PaymentFailed");
                    }
                    var blogIds = executedPayment.transactions[0].item_list.items[0].sku;

                    await HandleSuccessResponse(orderModel.Id, JsonConvert.SerializeObject(executedPayment));
                    _toast.AddSuccessToastMessage("Payment success");
                    //return View("PaymentSuccess");
                    return RedirectToAction("AddInitialSettings", "Settings");
                }
            }
            catch (Exception ex)
            {
                await HandleFailedResponse(orderModel.Id, ex.StackTrace, true);
                _toast.AddErrorToastMessage("Payment failed");
                return View("PaymentFailed");
            }
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }


        private async Task<OrderModel> CreateOrder(int planId, int employeeCount, PaymentGateway gateway)
        {
            var user = await _userManager.GetUserAsync(User);
            var plan = await _plan.GetById(planId);

            var planName = plan.Name;

            var guidd = Convert.ToString((new Random()).Next(100000));
            var guid = guidd;

            while (await _context.Orders.AnyAsync(x => x.OrderId == guid))
            {
                guidd = Convert.ToString((new Random()).Next(100000));
                guid = guidd;
            }

            var orderModel = new OrderModel
            {
                PlanId = plan.Id,
                TenantId = user.TenantId.Value,
                EmployeesCount = employeeCount,
                CreatedBy = user.Id,
                PerEmployeeAmount = plan.Price,
                Total = employeeCount * plan.Price,
                OrderId = guidd,
                PaymentGateway = gateway,
                Description = "An order for " + user.GetFullName() + " of plan " + plan.Name + " " + plan.Price.ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await _context.AddAsync(orderModel);
            await _context.SaveChangesAsync();

            return orderModel;
        }

        public async Task<IActionResult> SkipPayment(int? planId, int employeesCount)
        {
            if (planId == null)
                return NotFound();

            var orderModel = new OrderModel();

            var user = await _userManager.GetUserAsync(User);
            var plan = await _plan.GetById(planId.Value);

            var guidd = Convert.ToString((new Random()).Next(100000));

            while (await _context.Orders.AnyAsync(x => x.OrderId == guidd))
            {
                guidd = Convert.ToString((new Random()).Next(100000));
            }

            orderModel = new OrderModel
            {
                PlanId = plan.Id,
                TenantId = user.TenantId.Value,
                CreatedBy = user.Id,
                PerEmployeeAmount = plan.Price,
                Total = plan.Price * employeesCount,
                EmployeesCount = employeesCount,
                OrderId = guidd,
                Description = "An order for " + user.GetFullName() + " of plan " + plan.Name + " " + plan.Price.ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await _context.AddAsync(orderModel);
            await _context.SaveChangesAsync();

            await HandleSuccessResponse(orderModel.Id, "Auto verified for demo");

            _toast.AddSuccessToastMessage("Payment success");

            return RedirectToAction("AddInitialSettings", "Settings");
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private async Task<Payment> CreatePayment(APIContext apiContext, string redirectUrl, string blogId, string planDetails, string description, string totalAmount)
        {
            var currency = await _settings.GetCurrency();
            //create itemlist and add item objects to it  

            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = planDetails,
                currency = currency,
                price = totalAmount,
                quantity = "1",
                sku = "asd"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            //var details = new Details()
            //{
            //    tax = "1",
            //    shipping = "1",
            //    subtotal = "1"
            //};
            //Final amount with details  
            var amount = new Amount()
            {
                currency = currency,
                total = totalAmount, // Total must be equal to sum of tax, shipping and subtotal.  
                //details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = description,
                invoice_number = Guid.NewGuid().ToString(), //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

        private async Task HandleSuccessResponse(int orderId, string response)
        {
            var order = await _context.Orders
                .Include(x => x.Plan)
                .FirstOrDefaultAsync(x => x.Id == orderId);

            if (order != null)
            {
                order.Status = SAOrderStatus.Success;
                order.UpdatedAt = DateTime.Now;
                order.PaymentResponse = response;

                var tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == order.TenantId);

                if (tenant != null)
                {
                    //Update new plan and status
                    tenant.PlanId = order.PlanId;
                    tenant.SubscriptionStatus = SubscriptionStatusEnum.Active;
                    tenant.UpdatedAt = DateTime.Now;
                    tenant.TotalEmployeesCount += order.EmployeesCount;
                    tenant.AvailableEmployeesCount += order.EmployeesCount;
                    _context.Tenants.Update(tenant);
                    await _context.SaveChangesAsync();

                    //Create a subscription log
                    TenantSubscriptionModel subscription = new TenantSubscriptionModel
                    {
                        PlanId = order.PlanId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        StartDate = DateTime.Now,
                        OrderId = order.Id,
                        IsPromotional = order.Plan.IsPromotional,
                        CreatedBy = tenant.Id,
                        TenantId = tenant.Id,
                    };

                    if (order.Plan.Type == PlanType.Weekly)
                    {
                        subscription.EndDate = subscription.StartDate.AddDays(7);
                    }
                    else if (order.Plan.Type == PlanType.Monthly)
                    {
                        subscription.EndDate = subscription.StartDate.AddMonths(1);
                    }
                    else if (order.Plan.Type == PlanType.Yearly)
                    {
                        subscription.EndDate = subscription.StartDate.AddYears(1);
                    }

                    await _context.AddAsync(subscription);
                    await _context.SaveChangesAsync();


                }
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                var user = await _userManager.GetUserAsync(User);

                if (user != null && user.Status != ESSDataAccess.Enum.UserStatus.Active)
                {
                    user.Status = ESSDataAccess.Enum.UserStatus.Active;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

            }
        }

        private async Task HandleFailedResponse(int orderId, string message, bool isFailedWithException = false)
        {
            var order = await _context.Orders
               .Include(x => x.Plan)
               .FirstOrDefaultAsync(x => x.Id == orderId);
            if (order != null)
            {
                order.Status = isFailedWithException ? SAOrderStatus.FailedWException : SAOrderStatus.Failed;
                order.UpdatedAt = DateTime.Now;
                order.PaymentResponse = message;
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
        }


        private async Task<PlanModel> GetPlan(int planId)
        {
            return await _context.Plans.FirstOrDefaultAsync(x => x.Id == planId);
        }
    }
}
