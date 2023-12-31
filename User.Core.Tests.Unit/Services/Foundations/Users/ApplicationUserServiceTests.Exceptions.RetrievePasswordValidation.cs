// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using User.Core.Models.Users.Exceptions;
using User.Core.Models.Users;
using Xunit;
using FluentAssertions;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldThrowServiceExceptionOnRetrievePasswordValidationIfServiceErrorOccursAndLogItAsync()
        {
            // given
            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser();
            string somePassword = GetRandomPassword();
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
                broker.SelectPasswordValidationAsync(
                    randomApplicationUser, somePassword))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> retrieveApplicationUserPasswordValidationTask =
                this.applicationUserService.RetrieveUserPasswordValidationAsync(
                    randomApplicationUser, somePassword);

            ApplicationUserServiceException actualApplicationUserServiceException =
                await Assert.ThrowsAsync<ApplicationUserServiceException>(
                    retrieveApplicationUserPasswordValidationTask.AsTask);

            // then
            actualApplicationUserServiceException.Should().BeEquivalentTo(
                expectedApplicationUserServiceException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectPasswordValidationAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()),
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