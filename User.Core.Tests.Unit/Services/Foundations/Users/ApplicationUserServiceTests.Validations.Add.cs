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
        private async Task ShouldThrowValidationExceptionOnAddIfApplicationUserIsNullAndLogItAsync()
        {
            // given
            ApplicationUser nullApplicationUser = null;
            string somePassword = GetRandomPassword();
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
            ValueTask<ApplicationUser> addApplicationUserTask =
                this.applicationUserService.AddUserAsync(nullApplicationUser, somePassword);

            ApplicationUserValidationException actualApplicationUserValidationException =
                await Assert.ThrowsAnyAsync<ApplicationUserValidationException>(
                    addApplicationUserTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                    Times.Once());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        private async Task ShouldThrowValidationExceptionOnAddIfApplicationUserIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidApplicationUser = new ApplicationUser
            {
                UserName = invalidText,
            };

            var invalidApplicationUserException =
                new InvalidApplicationUserException(
                    message: "Invalid ApplicationUser, correct errors to continue.");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser),
                values: "Text is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.Id),
                values: "Id is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.UserName),
                values: "Text is required");

            invalidApplicationUserException.AddData(
               key: nameof(ApplicationUser.Email),
               values: "Text is required");

            invalidApplicationUserException.AddData(
               key: nameof(ApplicationUser.FirstName),
               values: "Text is required");

            invalidApplicationUserException.AddData(
               key: nameof(ApplicationUser.LastName),
               values: "Text is required");

            invalidApplicationUserException.AddData(
               key: nameof(ApplicationUser.PhoneNumber),
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
            ValueTask<ApplicationUser> addApplicationUserTask =
                this.applicationUserService.AddUserAsync(
                    invalidApplicationUser, GetRandomPassword());

            ApplicationUserValidationException actualApplicationUserValidationException =
                await Assert.ThrowsAnyAsync<ApplicationUserValidationException>(
                    addApplicationUserTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                    Times.Once());

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()),
                Times.Never());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnAddIfCreatedDateAndUpdatedDateNotTheSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            ApplicationUser randomApplicationUser = CreateRandomApplicationUser();
            ApplicationUser invalidApplicationUser = randomApplicationUser;
            invalidApplicationUser.CreatedDate.AddDays(randomNumber);

            var invalidApplicationUserException =
                new InvalidApplicationUserException(
                    message: "Invalid ApplicationUser, correct errors to continue.");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.UpdatedDate),
                values: $"Date is not the same as {nameof(ApplicationUser.CreatedDate)}.");

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: invalidApplicationUserException);

            // when
            ValueTask<ApplicationUser> addApplicationUserTask = 
                this.applicationUserService.AddUserAsync(
                    invalidApplicationUser, GetRandomPassword());

            ApplicationUserValidationException actualApplicationUserValidationException =
                await Assert.ThrowsAsync<ApplicationUserValidationException>(
                    addApplicationUserTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    actualApplicationUserValidationException))),
                    Times.Once);
            
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
