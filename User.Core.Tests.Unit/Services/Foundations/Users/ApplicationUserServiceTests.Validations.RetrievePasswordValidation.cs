// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using User.Core.Models.Users;
using User.Core.Models.Users.Exceptions;
using Xunit;

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        private async Task ShouldThrowValidationExceptionOnRetrievePasswordValidationIfNotValidAndLogItAsync(
            string invalidPasswordText)
        {
            // given
            var randomApplicationUser = CreateRandomApplicationUser();
            var somePassword = invalidPasswordText;

            var invalidApplicationUserPasswordException =
                new InvalidApplicationUserPasswordException(
                    message: "Invalid user password error occurred, provide valid password.");

            invalidApplicationUserPasswordException.AddData(
                key: nameof(somePassword),
                values: "Text is required");

            var expectedApplicationUserPasswordValidationException =
                new ApplicationUserPasswordValidationException(
                    message: "Password validation failed error, please provide a valid password.",
                    innerException: invalidApplicationUserPasswordException);

            // when
            ValueTask<bool> applicationUserPasswordValidationTask =
                this.applicationUserService.RetrieveUserPasswordValidationAsync(
                    randomApplicationUser, somePassword);

            ApplicationUserPasswordValidationException actualUserPasswordValidationException =
              await Assert.ThrowsAsync<ApplicationUserPasswordValidationException>(
                  applicationUserPasswordValidationTask.AsTask);

            // then
            actualUserPasswordValidationException.Should().BeEquivalentTo(
                expectedApplicationUserPasswordValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserPasswordValidationException))),
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