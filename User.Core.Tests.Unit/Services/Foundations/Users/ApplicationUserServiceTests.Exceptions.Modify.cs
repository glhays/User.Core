// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        private async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser(randomDateTimeOffset);

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
            ValueTask<ApplicationUser> modifyApplicationUserTask =
                this.applicationUserService.ModifyUserAsync(randomApplicationUser);

            ApplicationUserDependencyException actualApplicationUserDependencyException =
                await Assert.ThrowsAsync<ApplicationUserDependencyException>(() =>
                    modifyApplicationUserTask.AsTask());

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
                broker.SelectUserByIdAsync(randomApplicationUser.Id),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(randomApplicationUser),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}