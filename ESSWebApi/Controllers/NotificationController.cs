using CZ.Api.Base;
using ESSWebApi.Routes;
using ESSWebApi.Services.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESSWebApi.Controllers
{
    [Authorize, ApiController]
    public class NotificationController : BaseController
    {
        private readonly INotification _notification;
        public NotificationController(INotification notification)
        {
            _notification = notification;
        }

        [HttpPost(APIRoutes.Notification.SetasRead)]
        public async Task<IActionResult> SetasRead([FromBody] int? Id)
        {
            if (Id == null)
                BadRequest("Id Required");

            var result = await _notification.SetAsReaded(Id);
            if (!result)
                BadRequest("Invalid Id");

            return Ok();
        }

        [HttpGet(APIRoutes.Notification.GetNotifications)]
        public async Task<IActionResult> GetNotifications()
        {

            var result = await _notification.GetNotifications();
            return Ok(result);
        }

    }
}
