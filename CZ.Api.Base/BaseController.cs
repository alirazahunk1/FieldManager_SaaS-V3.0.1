using CZ.Api.Base.Extensions;
using CZ.Api.Base.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CZ.Api.Base
{
    public class BaseController : Controller
    {
        public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object? value)
        {
            var response = new CommonResponse
            {
                StatusCode = 400,
                Status = ResponseStatus.error,
                Data = value
            };

            return base.BadRequest(response);
        }


        public override OkObjectResult Ok([ActionResultObjectValue] object? value)
        {
            var response = new CommonResponse
            {
                StatusCode = 200,
                Status = ResponseStatus.success,
                Data = value
            };

            return base.Ok(response);
        }


        public override UnauthorizedObjectResult Unauthorized([ActionResultObjectValue] object? value)
        {
            var response = new CommonResponse
            {
                StatusCode = 401,
                Status = ResponseStatus.error,
                Data = value
            };

            return base.Unauthorized(response);
        }

        [NonAction]
        public int GetUserId()
        {
            return HttpContext.GetUserId();
        }
    }
}
