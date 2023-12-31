// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Xeptions;

namespace User.Core.Models.Users.Exceptions
{
    public class ApplicationUserPasswordValidationException : Xeption
    {
        public ApplicationUserPasswordValidationException(Xeption innerException)
            : base(message: "Password validation failed error, please provide a valid password.",
                  innerException: innerException)
        { }
        
        public ApplicationUserPasswordValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}