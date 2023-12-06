// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using System;
using User.Core.Models.Users.Exceptions;
using User.Core.Models.Users;
using Xunit;
using FluentAssertions;
using Microsoft.Data.SqlClient;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnRemoveByIdIfDbConcurrencyOccursAndLogItAsync()
        {
            // given
            Guid someGuid = Guid.NewGuid();

            var dbUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedApplicationUserException =
                new LockedApplicationUserException(
                    message: "ApplicationUser is currently locked, please try again.",
                    innerException: dbUpdateConcurrencyException);

            var expectedApplicationUserDependencyValidationException =
                new ApplicationUserDependencyValidationException(
                    message: "ApplicationUser dependency validation occurred, fix and try again.",
                    innerException: lockedApplicationUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(someGuid))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<ApplicationUser> removeApplicationUserByIdTask =
                this.applicationUserService.RemoveUserByIdAsync(someGuid);

            ApplicationUserDependencyValidationException actualApplicationUserDependencyValidationException =
                await Assert.ThrowsAsync<ApplicationUserDependencyValidationException>(
                    removeApplicationUserByIdTask.AsTask);

            // then
            actualApplicationUserDependencyValidationException.Should().BeEquivalentTo(
                expectedApplicationUserDependencyValidationException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once());
            
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserDependencyValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnRemoveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someGuid = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedApplicationUserStorageException =
                new FailedApplicationUserStorageException(
                    message: "Failed ApplicationUser storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedApplicationUserDependencyException =
                new ApplicationUserDependencyException(
                    message: "ApplicationUser dependency error occurred, contact support.",
                    innerException: failedApplicationUserStorageException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(someGuid))
                    .Throws(sqlException);

            // when
            ValueTask<ApplicationUser> removeApplicationUserByIdTask =
                this.applicationUserService.RemoveUserByIdAsync(someGuid);

            ApplicationUserDependencyException actualApplicationUserDependencyException =
                await Assert.ThrowsAsync<ApplicationUserDependencyException>(() =>
                    removeApplicationUserByIdTask.AsTask());

            // then
            actualApplicationUserDependencyException.Should().BeEquivalentTo(
                expectedApplicationUserDependencyException);
            
            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedApplicationUserDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}