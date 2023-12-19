// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace User.Core.Administration.Authorizations
{
    public static class PermissionProvider
    {
        public static List<Permission> GetAllPermissions()
        {
            return Enum.GetValues(typeof(Permission))
                .OfType<Permission>()
                .ToList();
        }
    }
}