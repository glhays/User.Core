// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class FailedApplicationUserStorageException : Xeption
    {
        public FailedApplicationUserStorageException(Exception innerException)
            : base(message: "Failed ApplicationUser storage error occurred, contact support.",
                 innerException: innerException)
        { }

        public FailedApplicationUserStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}