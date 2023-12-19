// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace User.Core.Administration.Authorizations
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionAuthorizationRequirement requirement)
        {
            var permissionClaim = context.User.FindFirst(
            c => c.Type == CustomClaimType.Permission);

            if (permissionClaim == null)
            {
                return Task.CompletedTask;
            }

            if (!int.TryParse(permissionClaim.Value, out int permissionClaimValue))
            {
                return Task.CompletedTask;
            }

            var userPermissions = (Permission)permissionClaimValue;

            if ((userPermissions & requirement.Permission) != 0)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}