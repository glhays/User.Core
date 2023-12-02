// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using User.Core.Models.Users;
using Force.DeepCloner;
using FluentAssertions;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldModifyApplicationUserAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            ApplicationUser randomApplicationUser =
                CreateRandomModifyApplicationUser(randomDateTimeOffset);

            ApplicationUser inputApplicationUser = randomApplicationUser;
            ApplicationUser updatedStorageApplicationUser = inputApplicationUser;
            ApplicationUser expectedApplicationUser = updatedStorageApplicationUser;

            ApplicationUser beforeUpdatedStorageApplicationUser =
                randomApplicationUser.DeepClone();

            inputApplicationUser.UpdatedDate = randomDateTimeOffset;
            Guid userId = inputApplicationUser.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(userId))
                    .ReturnsAsync(beforeUpdatedStorageApplicationUser);

            this.userManagementBrokerMock.Setup(broker =>
                broker.UpdateUserAsync(inputApplicationUser))
                    .ReturnsAsync(updatedStorageApplicationUser);

            // when
            ApplicationUser actualUser =
                await this.applicationUserService.ModifyUserAsync(
                    inputApplicationUser);

            // then
            actualUser.Should().BeEquivalentTo(expectedApplicationUser);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(userId),
                Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(inputApplicationUser),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}