// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class NullApplicationUserException : Xeption
    {
        public NullApplicationUserException()
            : base(
            message: "ApplicationUser is null, please fix and try again.")
        { }

        public NullApplicationUserException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}