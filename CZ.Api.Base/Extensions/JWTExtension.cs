using Microsoft.AspNetCore.Http;

namespace CZ.Api.Base.Extensions
{
    public static class JWTExtension
    {
        public static int GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return 0;
            }


            return Convert.ToInt32(httpContext.User.Claims.Single(x => x.Type == "id").Value);
        }
    }
}
