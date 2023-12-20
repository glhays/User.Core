// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class InvalidApplicationUserModifyPasswordException : Xeption
    {
        public InvalidApplicationUserModifyPasswordException()
            : base(message: "Invalid modify password occurred, correct errors to continue.")
        { }

        public InvalidApplicationUserModifyPasswordException(string message)
            : base(message)
        { }
    }
}