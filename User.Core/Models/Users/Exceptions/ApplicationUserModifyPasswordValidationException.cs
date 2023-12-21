// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class ApplicationUserModifyPasswordValidationException : Xeption
    {
        public ApplicationUserModifyPasswordValidationException(Xeption innerException)
            : base(
                 message: "User modify password validation errors occurred, please try again.",
                 innerException: innerException)
        { }

        public ApplicationUserModifyPasswordValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}