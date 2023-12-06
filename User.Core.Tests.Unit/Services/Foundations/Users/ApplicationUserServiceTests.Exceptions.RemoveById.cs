// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using System;
using User.Core.Models.Users.Exceptions;
using User.Core.Models.Users;
using Xunit;
using FluentAssertions;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnRemoveByIdIfDbConcurrencyOccursAndLogItAsync()
        {
            // given
            Guid someGuid = Guid.NewGuid();

            var dbUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedApplicationUserException =
                new LockedApplicationUserException(
                    message: "ApplicationUser is currently locked, please try again.",
                    innerException: dbUpdateConcurrencyException);

            var expectedApplicationUserDependencyValidationException =
                new ApplicationUserDependencyValidationException(
                    message: "ApplicationUser dependency validation occurred, fix and try again.",
                    innerException: lockedApplicationUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(someGuid))
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<ApplicationUser> removeApplicationUserByIdTask =
                this.applicationUserService.RemoveUserByIdAsync(someGuid);

            ApplicationUserDependencyValidationException actualApplicationUserDependencyValidationException =
                await Assert.ThrowsAsync<ApplicationUserDependencyValidationException>(
                    removeApplicationUserByIdTask.AsTask);

            // then
            actualApplicationUserDependencyValidationException.Should().BeEquivalentTo(
                expectedApplicationUserDependencyValidationException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once());
            
            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserDependencyValidationException))),
                        Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}