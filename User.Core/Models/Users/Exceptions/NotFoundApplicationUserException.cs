// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class NotFoundApplicationUserException : Xeption
    {
        public NotFoundApplicationUserException(Guid id)
                    : base(message: $"ApplicationUser not found with id: {id}.")
        { }

        public NotFoundApplicationUserException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}