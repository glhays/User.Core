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
        private async Task ShouldThrowValidationExceptionOnModifyIfApplicationUserIsNullAndLogItAsync()
        {
            // given
            ApplicationUser nullApplicationUser = null;
            var innerException = new Exception();

            var nullApplicationUserException =
                new NullApplicationUserException(
                    message: "ApplicationUser is null, please fix and try again.",
                    innerException: innerException);

            var expectApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: nullApplicationUserException);

            // when
            ValueTask<ApplicationUser> modifyApplicationUserTask =
                this.applicationUserService.ModifyUserAsync(nullApplicationUser);

            ApplicationUserValidationException actualApplicationUserValidationException =
                await Assert.ThrowsAsync<ApplicationUserValidationException>(
                    modifyApplicationUserTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectApplicationUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectApplicationUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<ApplicationUser>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        private async Task ShouldThrowValidationExceptionOnModifyIfApplicationUserIsInvalidAndLogItAsync(
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
                values: new[]
                {
                    "Date is required",
                    $"Date is the same as {nameof(ApplicationUser.CreatedDate)}"
                });

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: invalidApplicationUserException);

            // when
            ValueTask<ApplicationUser> modifyApplicationUserTask =
                this.applicationUserService.ModifyUserAsync(invalidApplicationUser);

            ApplicationUserValidationException actualApplicationUserValidationException =
               await Assert.ThrowsAsync<ApplicationUserValidationException>(
                   modifyApplicationUserTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }
    }
}