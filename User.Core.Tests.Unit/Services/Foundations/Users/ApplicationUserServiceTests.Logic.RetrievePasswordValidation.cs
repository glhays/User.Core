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
        private async Task ShouldValidateUserPasswordOnRetrieveAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            DateTimeOffset dateTime = randomDateTime;

            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser(dateTime);

            bool inValidPasswordStatus = false;
            bool somePasswordStatus = inValidPasswordStatus;
            bool expectedPasswordStatus = somePasswordStatus;
            string password = GetRandomPassword();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(dateTime);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectPasswordValidationAsync(randomApplicationUser, password))
                    .ReturnsAsync(expectedPasswordStatus);

            // when
            bool actualApplicationUserPasswordValidateTask =
                await this.applicationUserService.RetrieveUserPasswordValidationAsync(
                    randomApplicationUser, password);

            // then
            actualApplicationUserPasswordValidateTask.Should<bool>().Be(
                expectedPasswordStatus);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectPasswordValidationAsync(
                    It.IsAny<ApplicationUser>(),
                    It.IsAny<string>()),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}