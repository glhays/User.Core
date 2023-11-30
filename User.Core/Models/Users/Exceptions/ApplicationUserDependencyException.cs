// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class ApplicationUserDependencyException : Xeption
    {
        public ApplicationUserDependencyException(Exception innerException) :
            base(message: "ApplicationUser dependency error occurred, contact support.",
                innerException: innerException)
        { }

        public ApplicationUserDependencyException(string message, Exception innerException) :
            base(message, innerException)
        { }
    }
}
