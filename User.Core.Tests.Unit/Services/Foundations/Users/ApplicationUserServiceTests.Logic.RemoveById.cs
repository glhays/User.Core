// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using User.Core.Models.Users;
using Xunit;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldRemoveApplicationUserByIdAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser();

            ApplicationUser storageApplicationUser =
                randomApplicationUser;

            ApplicationUser inputApplicationUser =
                storageApplicationUser;

            ApplicationUser removedApplicationUser =
                inputApplicationUser;

            ApplicationUser expectedApplicationUser =
                removedApplicationUser.DeepClone();

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(someId))
                .ReturnsAsync(storageApplicationUser);

            this.userManagementBrokerMock.Setup(broker =>
                broker.DeleteUserAsync(inputApplicationUser))
                .ReturnsAsync(removedApplicationUser);

            // when
            ApplicationUser actualApplicationUser =
                await this.applicationUserService
                .RemoveUserByIdAsync(someId);

            // then
            actualApplicationUser.Should().BeEquivalentTo(
                expectedApplicationUser);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(someId),
                Times.Once());
        
            this.userManagementBrokerMock.Verify(broker =>
                broker.DeleteUserAsync(inputApplicationUser),
                Times.Once());
        
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}