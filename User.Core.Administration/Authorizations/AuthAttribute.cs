// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Microsoft.AspNetCore.Authorization;

namespace User.Core.Administration.Authorizations
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public AuthAttribute() { }

        public AuthAttribute(string policy) : base(policy) { }

        public AuthAttribute(Permission permission)
        {
            Permissions = permission;
        }

        public Permission Permissions
        {
            get
            {
                return !string.IsNullOrEmpty(Policy)
                    ? PolicyNameHelper.GetPermissionFrom(Policy)
                    : Permission.None;
            }
            set
            {
                Policy = value != Permission.None
                    ? PolicyNameHelper.GeneratePolicyNameFor(value)
                    : string.Empty;
            }
        }
    }
}