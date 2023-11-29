// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System.Threading.Tasks;
using User.Core.Models.Users.Exceptions;
using User.Core.Models.Users;
using Xeptions;

namespace User.Core.Services.Foundations.Users
{
    public partial class ApplicationUserService
    {
        private delegate ValueTask<ApplicationUser> ReturningApplicationUserFunction();

        private async ValueTask<ApplicationUser> TryCatch(
            ReturningApplicationUserFunction returningApplicationUserFunction)
        {
            try
            {
                return await returningApplicationUserFunction();
            }
            catch (NullApplicationUserException nullApplicationUserException)
            {
                throw CreateAndLogValidationException(nullApplicationUserException);
            }
            catch (InvalidApplicationUserException invalidApplicationUserException)
            {
                throw CreateAndLogValidationException(invalidApplicationUserException);
            }
        }

        private ApplicationUserValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var applicationUserValidationException =
                new ApplicationUserValidationException(exception);

            this.loggingBroker.LogError(applicationUserValidationException);

            return applicationUserValidationException;
        }
    }
}