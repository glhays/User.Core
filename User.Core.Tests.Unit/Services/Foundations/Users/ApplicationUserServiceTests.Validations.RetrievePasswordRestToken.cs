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
        private async void ShouldThrowValidationExceptionOnRetrievePasswordResetTokenIfUserIsNullAndLogItAsync()
        {
            // given
            ApplicationUser invalidApplicationUser = null;
            ApplicationUser inputApplicationUser = invalidApplicationUser;
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
            ValueTask<string> applicationUserPasswordResetTokenTask =
                this.applicationUserService.RetrieveUserPasswordResetTokenAsync(
                    inputApplicationUser);

            ApplicationUserValidationException actualUserValidationException =
              await Assert.ThrowsAsync<ApplicationUserValidationException>(
                  applicationUserPasswordResetTokenTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserPasswordResetTokenAsync(
                    It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        private async Task ShouldThrowValidationExceptionOnRetrievePasswordResetTokenIfUserInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidApplicationUser = new ApplicationUser
            {
                FirstName = invalidText,
                LastName = invalidText
            };

            var invalidApplicationUserException =
                new InvalidApplicationUserException(
                    message: "Invalid ApplicationUser, correct errors to continue.");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.FirstName),
                values: "Text is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.LastName),
                values: "Text is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.UserName),
                values: "Text is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.PhoneNumber),
                values: "Text is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.Email),
                values: "Text is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.CreatedDate),
                values: "Date is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.UpdatedDate),
                values: "Date is required");

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: invalidApplicationUserException);

            // when
            ValueTask<string> applicationUserPasswordResetTokenTask =
                this.applicationUserService.RetrieveUserPasswordResetTokenAsync(
                    invalidApplicationUser);

            ApplicationUserValidationException actualUserValidationException =
              await Assert.ThrowsAsync<ApplicationUserValidationException>(
                  applicationUserPasswordResetTokenTask.AsTask);

            // then
            actualUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserPasswordResetTokenAsync(
                    It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}