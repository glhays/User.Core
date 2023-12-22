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
        private async Task ShouldThrowServiceExceptionOnRetrievePasswordResetTokenIfServiceErrorOccursAndLogItAsync()
        {
            // given
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
                broker.SelectUserPasswordResetTokenAsync(
                    randomApplicationUser))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<string> retrieveUserPasswordRestTokenTask =
                this.applicationUserService.RetrieveUserPasswordResetTokenAsync(
                    randomApplicationUser);

            ApplicationUserServiceException actualApplicationUserServiceException =
                await Assert.ThrowsAsync<ApplicationUserServiceException>(
                    retrieveUserPasswordRestTokenTask.AsTask);

            // then
            actualApplicationUserServiceException.Should().BeEquivalentTo(
                expectedApplicationUserServiceException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserPasswordResetTokenAsync(
                    It.IsAny<ApplicationUser>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserServiceException))),
                        Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
