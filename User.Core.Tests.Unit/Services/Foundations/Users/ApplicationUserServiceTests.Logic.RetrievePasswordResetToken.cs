// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using User.Core.Models.Users;
using Xunit;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldRetrieveUserPasswordResetTokenAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            DateTimeOffset dateTimeOffset = randomDateTime;

            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser(dateTimeOffset);
            
            ApplicationUser inputApplicationUser = randomApplicationUser;
            ApplicationUser storageApplicationUser = randomApplicationUser;
            ApplicationUser expectedUser = storageApplicationUser;
            string randomToken = GetRandomString();
            string inputToken = randomToken;
            string expectedToken = inputToken;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(dateTimeOffset);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserPasswordResetTokenAsync(inputApplicationUser))
                    .ReturnsAsync(expectedToken);

            // when
            string actualToken =
                await this.applicationUserService.RetrieveUserPasswordResetTokenAsync(
                    inputApplicationUser);

            // then
            actualToken.Should().BeEquivalentTo(expectedToken);
            
            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserPasswordResetTokenAsync(inputApplicationUser),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
