// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using FluentAssertions;
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
        private async void ShouldThrowValidationExceptionOnModifyUserPasswordIfUserIsNullAndLogItAsync()
        {
            // given
            ApplicationUser invalidApplicationUser = null;
            string randomToken = GetRandomWord();
            string inputToken = randomToken;
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
            ValueTask<ApplicationUser> applicationUserTask =
                this.applicationUserService.ModifyUserPasswordAsync(
                    invalidApplicationUser, inputToken, inputPassword);

            ApplicationUserValidationException actualUserValidationException =
              await Assert.ThrowsAsync<ApplicationUserValidationException>(
                  applicationUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserPasswordAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        private async Task ShouldThrowValidationExceptionOnModifyPasswordIfInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            ApplicationUser randomApplicationUser = CreateRandomApplicationUser();
            var invalidPassword = invalidText;
            var invalidToken = invalidText;

            var invalidApplicationUserModifyPasswordException =
                new InvalidApplicationUserModifyPasswordException(
                    message: "Invalid modify password occurred, correct errors to continue.");

            invalidApplicationUserModifyPasswordException.AddData(
                key: nameof(invalidPassword),
                values: "Text is required");
            
            invalidApplicationUserModifyPasswordException.AddData(
                key: nameof(invalidToken),
                values: "Text is required");

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: invalidApplicationUserModifyPasswordException);

            // when
            ValueTask<ApplicationUser> applicationUserTask =
                this.applicationUserService.ModifyUserPasswordAsync(
                    randomApplicationUser, invalidToken, invalidPassword);

            ApplicationUserValidationException actualUserValidationException =
              await Assert.ThrowsAsync<ApplicationUserValidationException>(
                  applicationUserTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserPasswordAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                    Times.Never);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}