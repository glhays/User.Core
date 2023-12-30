// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class InvalidApplicationUserPasswordException : Xeption
    {
        public InvalidApplicationUserPasswordException()
            : base(message: "Invalid user password error occurred, provide valid password.")
        { }

        public InvalidApplicationUserPasswordException(string message)
            : base(message)
        { }
    }
}