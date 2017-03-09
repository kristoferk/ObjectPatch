using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Threading.Tasks;

namespace ExampleApiWeb.Framework
{
    public class CustomAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, object>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, object resource)
        {
            context.Succeed(requirement);
            return Task.FromResult(0);
        }
    }
}