// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class ApplicationUserDependencyValidationException : Xeption
    {
        public ApplicationUserDependencyValidationException(Xeption innerException)
            : base(
                message: "ApplicationUser dependency validation occurred, fix and try again.",
                innerException: innerException)
        { }

        public ApplicationUserDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}