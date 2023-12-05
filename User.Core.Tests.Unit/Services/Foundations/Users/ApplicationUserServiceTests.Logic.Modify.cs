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
            int minuteInPast = GetRandomNegativeNumber();
            
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            ApplicationUser randomApplicationUser =
                CreateRandomModifyApplicationUser(
                    randomDateTimeOffset.AddMinutes(minuteInPast));

            ApplicationUser inputApplicationUser =
                randomApplicationUser.DeepClone();

            inputApplicationUser.UpdatedDate = randomDateTimeOffset;

            ApplicationUser storageApplicationUser =
                randomApplicationUser;

            ApplicationUser updatedApplicationUser =
                inputApplicationUser;

            ApplicationUser expectedApplicationUser =
                updatedApplicationUser.DeepClone();

            Guid userId = inputApplicationUser.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputApplicationUser.Id))
                    .ReturnsAsync(storageApplicationUser);

            this.userManagementBrokerMock.Setup(broker =>
                broker.UpdateUserAsync(inputApplicationUser))
                    .ReturnsAsync(updatedApplicationUser);

            // when
            ApplicationUser actualApplicationUser =
                await this.applicationUserService.ModifyUserAsync(
                    inputApplicationUser);

            // then
            actualApplicationUser.Should().BeEquivalentTo(
                expectedApplicationUser);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(inputApplicationUser.Id),
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