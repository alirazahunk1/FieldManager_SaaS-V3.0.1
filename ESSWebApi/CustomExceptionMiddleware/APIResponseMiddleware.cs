
using ESSCommon.Core.Subscription;
using ESSDataAccess.Tenant;
using ESSWebApi.Dtos.Response;
using ESSWebApi.Logger;
using Microsoft.Extensions.Primitives;
using System.Net;

namespace ESSWebApi.CustomExceptionMiddleware
{
    public class APIResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITenant _tenant;
        private readonly ILoggerManager _logger;

        public APIResponseMiddleware(RequestDelegate next,
            ITenant tenant,
            ILoggerManager logger)
        {
            _logger = logger;
            _next = next;
            _tenant = tenant;
        }

        public async Task InvokeAsync(HttpContext httpContext, ISubscription subscription)
        {
            try
            {
                if (httpContext.Request.Headers.Authorization.Any())
                {
                    StringValues value = "";
                    var tenant = httpContext.Request.Headers
                     .TryGetValue("TenantId", out value);

                    if (tenant)
                    {
                        _tenant.SetTenant(Convert.ToInt32(value));
                        if (await subscription.IsSubscriptionActive(_tenant.TenantId.Value))
                        {
                            await _next(httpContext);
                        }
                        else
                        {
                            await HandleSubscriptionExpireAsync(httpContext);
                        }
                    }
                    else
                    {
                        await HandleTenantErrorAsync(httpContext);
                    }

                }
                else
                {
                    await _next(httpContext);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ErrorResponse()
            {
                StatusCode = context.Response.StatusCode,
                Status = "Error",
                Data = "Internal Server Error from the custom middleware."
            }.ToString());
        }

        private async Task HandleTenantErrorAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(new ErrorResponse()
            {
                StatusCode = context.Response.StatusCode,
                Status = "Error",
                Data = "Tenant id is required."
            }.ToString());
        }

        private async Task HandleSubscriptionExpireAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(new ErrorResponse()
            {
                StatusCode = context.Response.StatusCode,
                Status = "Subscription Error",
                Data = "Your subscription is expired"
            }.ToString());
        }
    }
}
