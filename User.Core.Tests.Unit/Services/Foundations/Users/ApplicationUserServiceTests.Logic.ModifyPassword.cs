// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using User.Core.Models.Users;
using Xunit;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldModifyUserPasswordAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            DateTimeOffset dateTime = randomDateTime;
            ApplicationUser randomApplicationUser = CreateRandomApplicationUser(dateTime);
            ApplicationUser inputApplicationUser = randomApplicationUser;
            ApplicationUser storageApplicationUser = randomApplicationUser;
            ApplicationUser expectedApplicationUser = storageApplicationUser;
            string password = GetRandomPassword();
            string token = GetRandomPassword();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(dateTime);

            this.userManagementBrokerMock.Setup(broker =>
                broker.UpdateUserPasswordAsync(inputApplicationUser, token, password))
                    .ReturnsAsync(IdentityResult.Success);

            // when
            ApplicationUser actualApplicationUser =
                await this.applicationUserService.ModifyUserPasswordAsync(
                    inputApplicationUser, token, password);

            // then
            actualApplicationUser.Should().BeEquivalentTo(expectedApplicationUser);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserPasswordAsync(inputApplicationUser, token, password),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}