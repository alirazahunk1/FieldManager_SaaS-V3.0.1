using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NToastNotify;

namespace ESSWebPortal.Controllers
{
    public class BaseController : Controller
    {
        private readonly IToastNotification _toast;
        protected string CookieTenant => Request.Cookies["Tenant"] ?? "";
        public BaseController(IToastNotification toast)
        {
            _toast = toast;
        }
        public override async void OnActionExecuted(ActionExecutedContext context)
        {

            base.OnActionExecuted(context);
        }

        public void ToastSuccessMessage(string msg)
        {
            _toast.AddSuccessToastMessage(msg);
        }

        public void ToastItemAddedMsg()
        {
            _toast.AddSuccessToastMessage("Item added successfully");
        }

        public void ToastItemAddedMsg(string msg)
        {
            _toast.AddSuccessToastMessage(msg);
        }

        public void ToastItemEditMsg()
        {
            _toast.AddSuccessToastMessage("Item updated successfully");
        }

        public void ToastErrorMessage(string msg)
        {
            _toast.AddErrorToastMessage(msg);
        }

        public void ToastValidationErrorMsg()
        {
            _toast.AddErrorToastMessage("Please clear the validation errors");
        }

    }
}
