using ESSWebApi.CustomExceptionMiddleware;

namespace ESSWebApi.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
       
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<APIResponseMiddleware>();
        }
    }
}
