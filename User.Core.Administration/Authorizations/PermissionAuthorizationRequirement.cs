// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Microsoft.AspNetCore.Authorization;

namespace User.Core.Administration.Authorizations
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public PermissionAuthorizationRequirement(Permission permission)
        {
            Permission = permission;
        }
        public Permission Permission { get; private set; }
    }
}