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
        private async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            var invalidApplicationUserId = Guid.Empty;

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
            ValueTask<ApplicationUser> retrieveByIdApplicationUserTask =
                this.applicationUserService.RetrieveUserByIdAsync(
                    invalidApplicationUserId);

            ApplicationUserValidationException actualApplicationUserValidationException =
                await Assert.ThrowsAsync<ApplicationUserValidationException>(
                    retrieveByIdApplicationUserTask.AsTask);

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
