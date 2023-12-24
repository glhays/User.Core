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
        private async Task ShouldThrowValidationExceptionOnRetrievePasswordValidationIfUserIsNullAndLogItAsync()
        {
            // given
            ApplicationUser invalidApplicationUser = null;
            string randomPassword = GetRandomPassword();
            string inputPassword = randomPassword;
            var innerException = new Exception();

            var nullApplicationUserException =
                new NullApplicationUserException(
                    message: "ApplicationUser is null, please fix and try again.",
                    innerException: innerException);

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: nullApplicationUserException);

            // when
            ValueTask<bool> applicationUserPasswordValidationTask =
                this.applicationUserService.RetrieveUserPasswordValidationAsync(
                    invalidApplicationUser, inputPassword);

            ApplicationUserValidationException actualUserValidationException =
              await Assert.ThrowsAsync<ApplicationUserValidationException>(
                  applicationUserPasswordValidationTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectPasswordValidationAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}