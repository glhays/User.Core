// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using User.Core.Models.Users;
using User.Core.Models.Users.Exceptions;
using Xunit;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependenyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ApplicationUser someApplicationUser = CreateRandomApplicationUser();
            SqlException sqlException = GetSqlException();

            var failedApplicationUserStorageException =
                new FailedApplicationUserStorageException(
                    message: "Failed ApplicationUser storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedApplicationUserDependencyException =
                new ApplicationUserDependencyException(
                    message: "ApplicationUser dependency error occurred, contact support.",
                innerException: failedApplicationUserStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<ApplicationUser> addApllicationUserTask =
                this.applicationUserService.AddUserAsync(someApplicationUser, GetRandomPassword());

            ApplicationUserDependencyException actualApplicationUserDependencyException =
                await Assert.ThrowsAsync<ApplicationUserDependencyException>(
                    addApllicationUserTask.AsTask);

            // then
            actualApplicationUserDependencyException.Should().BeEquivalentTo(
                expectedApplicationUserDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedApplicationUserDependencyException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnAddIfApplicationUserAlreadyExistsAndLogItAsync()
        {
            // given
            ApplicationUser randomApplicationUser = CreateRandomApplicationUser();
            ApplicationUser alreadyExistsUser = randomApplicationUser;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsUserException =
                new AlreadyExistsApplicationUserException(
                    message: "ApplicationUser already exists with this id.",
                    innerException: duplicateKeyException);

            var expectedApplicationUserDependencyValidationException =
                new ApplicationUserDependencyValidationException(
                    message: "ApplicationUser dependency validation occurred, fix and try again.",
                    innerException: alreadyExistsUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<ApplicationUser> addApplicationUserTask =
                this.applicationUserService.AddUserAsync(
                    randomApplicationUser, GetRandomPassword());

            ApplicationUserDependencyValidationException actualApplicationUserDependencyValidationException =
               await Assert.ThrowsAsync<ApplicationUserDependencyValidationException>(
                   addApplicationUserTask.AsTask);

            // then
            actualApplicationUserDependencyValidationException.Should().BeEquivalentTo(
                expectedApplicationUserDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserDependencyValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(
                    It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}
