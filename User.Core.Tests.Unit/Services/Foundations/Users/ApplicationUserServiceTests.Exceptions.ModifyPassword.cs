// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using User.Core.Models.Users;
using User.Core.Models.Users.Exceptions;
using Xunit;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnModifyPasswordIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser(randomDateTimeOffset);

            var someToken = GetRandomWord();
            var somePassword = GetRandomPassword();

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
                broker.UpdateUserPasswordAsync(
                    randomApplicationUser, someToken, somePassword))
                    .ThrowsAsync(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<ApplicationUser> userModifyPasswordTask =
                this.applicationUserService.ModifyUserPasswordAsync(
                    randomApplicationUser, someToken, somePassword);

            ApplicationUserDependencyException actualApplicationUserDependencyException =
                await Assert.ThrowsAsync<ApplicationUserDependencyException>(() =>
                    userModifyPasswordTask.AsTask());

            // then
            actualApplicationUserDependencyException.Should().BeEquivalentTo(
                expectedApplicationUserDependencyException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserPasswordAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
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

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnModifyPasswordIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            var someToken = GetRandomWord();
            var somePassword = GetRandomPassword();

            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser();

            var dbUpdateException = new DbUpdateException();

            var failedApplicationUserException =
                new FailedApplicationUserStorageException(
                    message: "Failed ApplicationUser storage error occurred, contact support.",
                    innerException: dbUpdateException);

            var expectedApplicationUserDependencyException =
                new ApplicationUserDependencyException(
                    message: "ApplicationUser dependency error occurred, contact support.",
                    innerException: failedApplicationUserException);


            this.userManagementBrokerMock.Setup(broker =>
                broker.UpdateUserPasswordAsync(
                    randomApplicationUser, someToken, somePassword))
                    .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<ApplicationUser> userModifyPasswordTask =
                this.applicationUserService.ModifyUserPasswordAsync(
                    randomApplicationUser , someToken, somePassword);

            ApplicationUserDependencyException actualApplicationUserDependencyException =
                await Assert.ThrowsAsync<ApplicationUserDependencyException>(
                    userModifyPasswordTask.AsTask);

            // then
            actualApplicationUserDependencyException.Should().BeEquivalentTo(
                expectedApplicationUserDependencyException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserPasswordAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
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
        private async Task ShouldThrowServiceExceptionOnModifyPasswordIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var someToken = GetRandomWord();
            var somePassword = GetRandomPassword();

            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser();

            var serviceException = new Exception();

            var failedApplicationUserException =
                new FailedApplicationUserServiceException(
                    message: "ApplicationUser service failure occurred, please contact support",
                    innerException: serviceException);

            var expectedApplicationUserServiceException =
                new ApplicationUserServiceException(
                    message: "ApplicationUser service error occurred, contact support.",
                    innerException: failedApplicationUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.UpdateUserPasswordAsync(
                    randomApplicationUser, someToken, somePassword))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<ApplicationUser> modifyApplicationUserTask =
                this.applicationUserService.ModifyUserPasswordAsync(
                    randomApplicationUser, someToken, somePassword);

            ApplicationUserServiceException actualApplicationUserServiceException =
                await Assert.ThrowsAsync<ApplicationUserServiceException>(
                    modifyApplicationUserTask.AsTask);

            // then
            actualApplicationUserServiceException.Should().BeEquivalentTo(
                expectedApplicationUserServiceException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserPasswordAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserServiceException))),
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
