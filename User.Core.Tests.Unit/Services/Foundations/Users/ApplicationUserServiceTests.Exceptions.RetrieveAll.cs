// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using User.Core.Models.Users.Exceptions;
using Xunit;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogItAsync()
        {
            // given
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
                broker.SelectAllUsers())
                    .Throws(sqlException);

            // when
            Action retrieveAllApplicationUsersAction = () =>
            this.applicationUserService.RetrieveAllUsers();

            ApplicationUserDependencyException actualUserStorageDependencyException =
                Assert.Throws<ApplicationUserDependencyException>(
                    retrieveAllApplicationUsersAction);

            // then
            actualUserStorageDependencyException.Should()
                .BeEquivalentTo(expectedApplicationUserDependencyException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectAllUsers(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedApplicationUserDependencyException))),
                    Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}