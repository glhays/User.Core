// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

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
        private async Task ShouldValidateUserPasswordOnRetrieveAsync()
        {
            // given
            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser();

            string inputPassword = GetRandomPassword();
            randomApplicationUser.Password = inputPassword;
            bool expectedPasswordStatus = true;

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectPasswordValidationAsync(randomApplicationUser, inputPassword))
                    .ReturnsAsync(expectedPasswordStatus);

            // when
            bool actualApplicationUserPasswordValidateTask =
                await this.applicationUserService.RetrieveUserPasswordValidationAsync(
                    randomApplicationUser, randomApplicationUser.Password);

            // then
            actualApplicationUserPasswordValidateTask.Should().Be(expectedPasswordStatus);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectPasswordValidationAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()),
                    Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}