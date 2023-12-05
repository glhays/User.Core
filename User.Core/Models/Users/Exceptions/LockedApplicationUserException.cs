// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class LockedApplicationUserException : Xeption
    {
        public LockedApplicationUserException(Exception innerException)
            : base(
                message: "ApplicationUser is currently locked, please try again.",
                innerException: innerException)
        { }

        public LockedApplicationUserException(string message, Exception innerException)
            : base(message: message, innerException: innerException)
        { }
    }
}
