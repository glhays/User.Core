// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace User.Core.Administration.Authorizations
{
    public static class IAuthorizationServiceExtensions
    {
        public static Task<AuthorizationResult> AuthorizeAsync(
            this IAuthorizationService service,
            ClaimsPrincipal user,
            Permission permission)
        {
            return service.AuthorizeAsync(user, PolicyNameHelper.GeneratePolicyNameFor(permission));
        }
    }
}