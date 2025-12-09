using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HelpTrack.Helpers
{
    public class RequireAdminAttribute : AuthorizeAttribute, IAuthorizationRequirement
    {
        public RequireAdminAttribute()
        {
            Policy = "RequireAdminRole";
        }
    }

    public class RequireAdminHandler : AuthorizationHandler<RequireAdminAttribute>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            RequireAdminAttribute requirement)
        {
            if (context.User == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var roleClaim = context.User.FindFirst(ClaimTypes.Role);
            if (roleClaim != null && roleClaim.Value == "Administrador")
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
