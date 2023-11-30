// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class ApplicationUserServiceException : Xeption
    {
        public ApplicationUserServiceException(Exception innerException)
            : base(
                message: "ApplicationUser service error occurred, contact support.",
                innerException: innerException)
        { }

        public ApplicationUserServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}