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
using Xeptions;
using Xunit;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidApplicationUserId = Guid.Empty;

            var invalidApplicationUserException =
                new InvalidApplicationUserException(
                    message: "Invalid ApplicationUser, correct errors to continue.");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.Id),
                values: "Id is required");

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: invalidApplicationUserException);

            // when
            ValueTask<ApplicationUser> removeApplicationUserByIdTask =
                this.applicationUserService.RemoveUserByIdAsync(invalidApplicationUserId);

            // then
            await Assert.ThrowsAsync<ApplicationUserValidationException>(() =>
               removeApplicationUserByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.DeleteUserAsync(It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnRemoveByIdIfNotFoundAndLogItAsync()
        {
            // given
            Guid someApplicationUserId = Guid.NewGuid();
            ApplicationUser nullApplicationUser = null;
            var innerException = new Exception();

            var notFoundApplicationUserException =
                new NotFoundApplicationUserException(
                    message: $"ApplicationUser not found with id: {someApplicationUserId}.",
                    innerException: innerException.InnerException.As<Xeption>());

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: notFoundApplicationUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(nullApplicationUser);

            // when
            ValueTask<ApplicationUser> removeApplicationUserByIdTask =
                this.applicationUserService.RetrieveUserByIdAsync(someApplicationUserId);

            ApplicationUserValidationException actualApplicationUserValidationException =
                await Assert.ThrowsAsync<ApplicationUserValidationException>(
                    removeApplicationUserByIdTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                        Times.Once());

            this.userManagementBrokerMock.Verify(broker =>
                broker.DeleteUserAsync(It.IsAny<ApplicationUser>()),
                    Times.Never);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}