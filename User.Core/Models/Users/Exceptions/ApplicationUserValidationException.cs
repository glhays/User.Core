// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class ApplicationUserValidationException : Xeption
    {
        public ApplicationUserValidationException(Xeption innerException)
            :base(
                 message: "ApplicationUser validation errors occurred, please try again.",
                 innerException: innerException)
        { }
        
        public ApplicationUserValidationException(string message, Xeption innerException)
            :base(message, innerException)
        { }
    }
}