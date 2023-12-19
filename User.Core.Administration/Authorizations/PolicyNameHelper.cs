// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;

namespace User.Core.Administration.Authorizations
{
    public static class PolicyNameHelper
    {
        public const string Prefix = "Permission";

        public static bool IsValidPolicyName(string policyName)
        {
            return policyName != null && policyName.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase);
        }

        public static string GeneratePolicyNameFor(Permission permission)
        {
            return $"{Prefix}{(int)permission}";
        }

        public static Permission GetPermissionFrom(string policyName)
        {
            var permissionValue = int.Parse(policyName[Prefix.Length..]!);

            return (Permission)permissionValue;
        }
    }
}