using ESSDataAccess;
using ESSDataAccess.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;

namespace ESSWebPortal.ACL
{
    public class DynamicAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly AppDbContext _dbContext;

        public DynamicAuthorizationFilter(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            if (HasAllowAnonymous(context))
            {
                return;
            }

            if (!IsUserAuthenticated(context))
            {
                context.Result = new UnauthorizedResult();
                return;
            }



            var actionId = GetActionId(context);
            var userName = context.HttpContext.User.Identity.Name;

            if (userName.Equals("superadmin"))
            {
                return;
            }

            var roles = await (
                from user in _dbContext.Users
                join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                join role in _dbContext.Roles on userRole.RoleId equals role.Id
                where user.UserName == userName
                select role
            ).ToListAsync();

            if (roles.Any(x => x.Name == UserRoles.Admin))
            {
                return;
            }

            foreach (var role in roles)
            {
                if (string.IsNullOrEmpty(role.Access))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                var accessList = JsonConvert.DeserializeObject<IEnumerable<MvcControllerInfo>>(role.Access);
                if (accessList.SelectMany(c => c.Actions).Any(a => a.Id == actionId))
                    return;
            }

            context.Result = new ForbidResult();
        }

        private bool IsProtectedAction(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                return false;

            var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var controllerTypeInfo = controllerActionDescriptor.ControllerTypeInfo;
            var actionMethodInfo = controllerActionDescriptor.MethodInfo;

            var authorizeAttribute = controllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>();
            if (authorizeAttribute != null)
                return true;

            authorizeAttribute = actionMethodInfo.GetCustomAttribute<AuthorizeAttribute>();
            if (authorizeAttribute != null)
                return true;

            return false;
        }

        private bool IsUserAuthenticated(AuthorizationFilterContext context)
        {
            return context.HttpContext.User.Identity.IsAuthenticated;
        }

        private bool HasAllowAnonymous(AuthorizationFilterContext context)
        {
            var filters = context.Filters;
            for (var i = 0; i < filters.Count; i++)
            {
                if (filters[i] is IAllowAnonymousFilter)
                {
                    return true;
                }
            }

            // When doing endpoint routing, MVC does not add AllowAnonymousFilters for 
            // AllowAnonymousAttributes that were discovered on controllers and actions. To maintain 
            // compatibility with 2.x, we'll check for the presence of IAllowAnonymous in endpoint metadata.
            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return true;
            }

            return false;
        }

        private string GetActionId(AuthorizationFilterContext context)
        {
            var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            var area = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue;
            var controller = controllerActionDescriptor.ControllerName;
            var action = controllerActionDescriptor.ActionName;

            return $"{area}:{controller}:{action}";
        }
    }

}
