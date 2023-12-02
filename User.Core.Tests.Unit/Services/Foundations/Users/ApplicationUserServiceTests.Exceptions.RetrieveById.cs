// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Threading.Tasks;
using User.Core.Models.Users;
using User.Core.Models.Users.Exceptions;
using Xunit;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someApplicationUserId = Guid.NewGuid();
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
                broker.SelectUserByIdAsync(someApplicationUserId))
                .ThrowsAsync(sqlException);

            // when
            ValueTask<ApplicationUser> retrieveUserTask =
                this.applicationUserService.RetrieveUserByIdAsync(
                    someApplicationUserId);

            ApplicationUserDependencyException actualUserDependencyException =
                await Assert.ThrowsAsync<ApplicationUserDependencyException>(
                    retrieveUserTask.AsTask);

            // then
            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

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

        [Fact]
        private async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someApplicationUserId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedApplicationUserServiceException =
                new FailedApplicationUserServiceException(
                    message: "ApplicationUser service failure occurred, please contact support",
                    innerException: serviceException);

            var expectedApplicationUserServiceException =
                new ApplicationUserServiceException(
                    message: "ApplicationUser service error occurred, contact support.",
                    innerException: failedApplicationUserServiceException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ApplicationUser> retrieveApplicationUserByIdTask =
                this.applicationUserService.RetrieveUserByIdAsync(
                    someApplicationUserId);

            ApplicationUserServiceException actualApplicationUserServiceException =
                await Assert.ThrowsAsync<ApplicationUserServiceException>(
                    retrieveApplicationUserByIdTask.AsTask);

            // then
            actualApplicationUserServiceException.Should().BeEquivalentTo(
                expectedApplicationUserServiceException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserServiceException))),
                        Times.Once());

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}