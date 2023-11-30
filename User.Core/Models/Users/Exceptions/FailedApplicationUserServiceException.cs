// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class FailedApplicationUserServiceException : Xeption
    {
        public FailedApplicationUserServiceException(Exception innerException)
            : base(message: "ApplicationUser service failure occurred, please contact support",
                innerException: innerException)
        { }

        public FailedApplicationUserServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}