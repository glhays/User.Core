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
    }
}