// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class InvalidApplicationUserException : Xeption
    {
        public InvalidApplicationUserException()
            : base(message: "Invalid ApplicationUser, correct errors to continue.")
        { }

        public InvalidApplicationUserException(string message)
            : base(message)
        { }
    }
}
