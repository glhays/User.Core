// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

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
        private async Task ShouldRetrieveApplicationUserById()
        {
            //given
            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser();

            ApplicationUser storageApplicationUser =
                randomApplicationUser;

            ApplicationUser expectedApplicationUser =
                storageApplicationUser.DeepClone();

            this.userManagementBrokerMock.Setup(broker =>
                    broker.SelectUserByIdAsync(
                        randomApplicationUser.Id))
                        .ReturnsAsync(storageApplicationUser);

            //when 
            ApplicationUser actualApplicationUser =
                await this.applicationUserService.RetrieveUserByIdAsync(
                    randomApplicationUser.Id);

            //then
            actualApplicationUser.Should().BeEquivalentTo(
                expectedApplicationUser);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(randomApplicationUser.Id),
                Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}