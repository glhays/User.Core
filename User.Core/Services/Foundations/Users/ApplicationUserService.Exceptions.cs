// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Linq;
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
        private delegate ValueTask<string> ReturningApplicationUserStringFunction();
        private delegate IQueryable<ApplicationUser> ReturningApplicationUsersFunction();
        private delegate ValueTask<bool> ReturningApplicationUserBooleanFunction();

        private async ValueTask<bool> TryCatch(
            ReturningApplicationUserBooleanFunction returningApplicationUserBooleanFunction)
        {
            try
            {
                return await returningApplicationUserBooleanFunction();
            }
            catch (NullApplicationUserException nullApplicationUserException)
            {
                throw CreateAndLogValidationException(nullApplicationUserException);
            }
            catch (ApplicationUserPasswordValidationException applicationUserPasswordValidationException)
            {
                throw CreateAndLogValidationException(applicationUserPasswordValidationException);
            }
            catch(InvalidApplicationUserPasswordException invalidApplicationUserPasswordException)
            {
                throw CreateAndLogPasswordValidationException(invalidApplicationUserPasswordException);
            }
        }

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
            catch (InvalidApplicationUserModifyPasswordException invalidApplicationUserModifyPasswordException)
            {
                throw CreateAndLogModifyPasswordValidationException(invalidApplicationUserModifyPasswordException);
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
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedApplicationUserException =
                    new LockedApplicationUserException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedApplicationUserException);
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
        
        private async ValueTask<string> TryCatch(
            ReturningApplicationUserStringFunction returningApplicationUserStringFunction)
        {
            try
            {
                return await returningApplicationUserStringFunction();
            }
            catch (NullApplicationUserException nullApplicationUserException)
            {
                throw CreateAndLogValidationException(nullApplicationUserException);
            }
            catch (InvalidApplicationUserException invalidApplicationUserException)
            {
                throw CreateAndLogValidationException(invalidApplicationUserException);
            }
            catch (InvalidApplicationUserModifyPasswordException invalidApplicationUserModifyPasswordException)
            {
                throw CreateAndLogModifyPasswordValidationException(invalidApplicationUserModifyPasswordException);
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
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedApplicationUserException =
                    new LockedApplicationUserException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedApplicationUserException);
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

        private IQueryable<ApplicationUser> TryCatch(
            ReturningApplicationUsersFunction returningApplicationUsersFunction)
        {
            try
            {
                return returningApplicationUsersFunction();
            }
            catch (SqlException sqlException)
            {
                var failedApplicationUserStorageException =
                    new FailedApplicationUserStorageException(sqlException);

                throw CreateAndLogCriticalDependencyExcpetion(
                    failedApplicationUserStorageException);
            }
            catch (Exception exception)
            {
                var failedApplicationUsersServiceException =
                    new FailedApplicationUserServiceException(exception);

                throw CreateAndLogServiceException(failedApplicationUsersServiceException);
            }
        }

        private ApplicationUserPasswordValidationException CreateAndLogPasswordValidationException(
            Xeption exception)
        {
            var applicationUserPasswordValidationException =
                new ApplicationUserPasswordValidationException(exception);

            this.loggingBroker.LogError(applicationUserPasswordValidationException);

            return applicationUserPasswordValidationException;
        }

        private ApplicationUserModifyPasswordValidationException CreateAndLogModifyPasswordValidationException(
            Xeption exception)
        {
            var applicationUserModifyPasswordValidationException =
                new ApplicationUserModifyPasswordValidationException(exception);

            this.loggingBroker.LogError(applicationUserModifyPasswordValidationException);

            return applicationUserModifyPasswordValidationException;
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