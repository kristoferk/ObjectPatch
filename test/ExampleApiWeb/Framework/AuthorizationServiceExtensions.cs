using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ExampleApiWeb.Code
{
    public static class AuthorizationServiceExtensions
    {
        public static async Task<bool> AssertAuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user, object resource, IAuthorizationRequirement requirements)
        {
            bool result = await service.AuthorizeAsync(user, resource, requirements);
            if (!result)
            {
                throw new CustomHttpException(HttpStatusCode.BadRequest, "Access denied");
            }

            return true;
        }
    }

    public static class Requirements
    {
        public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = "Create" };
        public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = "Read" };
        public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = "Update" };
        public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = "Delete" };
    }
}
