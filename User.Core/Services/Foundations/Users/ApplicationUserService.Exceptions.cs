// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using User.Core.Models.Users;
using User.Core.Models.Users.Exceptions;
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
            catch (SqlException sqlException)
            {
                var failedApplicationUserStorageException =
                    new FailedApplicationUserStorageException(sqlException);

                throw CreateAndLogCriticalDependencyExcpetion(
                    failedApplicationUserStorageException);
            }
            catch (NotFoundApplicationUserException notFoundApplicationUserException)
            {
                throw CreateAndLogValidationException(notFoundApplicationUserException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsApplicationUserException =
                    new AlreadyExistsApplicationUserException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsApplicationUserException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedApplicationUserStorageException =
                    new FailedApplicationUserStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedApplicationUserStorageException);
            }
            catch (Exception exception)
            {
                var failedApplicationUserServiceException =
                    new FailedApplicationUserServiceException(exception);

                throw CreateAndLogServiceException(failedApplicationUserServiceException);
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

        private ApplicationUserDependencyException CreateAndLogCriticalDependencyExcpetion(
            Xeption exception)
        {
            var applicationUserDependencyException =
                new ApplicationUserDependencyException(exception);

            this.loggingBroker.LogCritical(applicationUserDependencyException);

            return applicationUserDependencyException;
        }

        private ApplicationUserDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var applicationUserDependencyValidationException =
                new ApplicationUserDependencyValidationException(exception);
            
            this.loggingBroker.LogError(applicationUserDependencyValidationException);
            
            return applicationUserDependencyValidationException;
        }

        private ApplicationUserDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var applicationUserDependencyException =
                new ApplicationUserDependencyException(exception);

            this.loggingBroker.LogError(applicationUserDependencyException);

            return applicationUserDependencyException;
        }

        private Exception CreateAndLogServiceException(Xeption exception)
        {
            var applicationUserServiceException = new ApplicationUserServiceException(exception);
            this.loggingBroker.LogError(applicationUserServiceException);

            return applicationUserServiceException;
        }
    }
}