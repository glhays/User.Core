// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class AlreadyExistsApplicationUserException : Xeption
    {
        public AlreadyExistsApplicationUserException(Exception innerException)
            : base(
                message: "ApplicationUser already exists with this id.",
                innerException: innerException)
        { }

        public AlreadyExistsApplicationUserException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}