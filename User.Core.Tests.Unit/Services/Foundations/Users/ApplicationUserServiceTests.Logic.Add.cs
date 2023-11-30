// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.AspNetCore.Identity;
using Moq;
using User.Core.Models.Users;
using Xunit;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldAddApplicationUserAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset dateTimeOffset = randomDateTimeOffset;
            ApplicationUser randomApplicationUser = CreateRandomApplicationUser(dateTimeOffset);
            ApplicationUser inputApplicationUser = randomApplicationUser;
            ApplicationUser storageApplicationUser = inputApplicationUser;
            ApplicationUser expectedApplicationUser = storageApplicationUser.DeepClone();
            string inputPassword = GetRandomString();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.userManagementBrokerMock.Setup(broker =>
                broker.InsertUserAsync(inputApplicationUser, inputPassword))
                .ReturnsAsync(IdentityResult.Success);

            // when
            ApplicationUser actualApplicationUser =
                await this.applicationUserService.AddUserAsync(
                    inputApplicationUser, inputPassword);

            // then
            actualApplicationUser.Should().BeEquivalentTo(expectedApplicationUser);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.userManagementBrokerMock.Verify(broker =>
                broker.InsertUserAsync(inputApplicationUser, inputPassword),
                Times.Once());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}