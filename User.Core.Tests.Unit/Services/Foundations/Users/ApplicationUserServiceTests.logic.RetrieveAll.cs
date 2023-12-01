// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Linq;
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
        private async Task ShouldReturnAllApplicationUsers()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            IQueryable<ApplicationUser> randomApplicationUsers =
                CreateRandomApplicationUsers(randomDateTimeOffset);

            IQueryable<ApplicationUser> storageApplicationUsers =
                randomApplicationUsers;
            
            IQueryable<ApplicationUser> expectedApplicationUsers =
                storageApplicationUsers;

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectAllUsers())
                .Returns(storageApplicationUsers);

            // when
            IQueryable<ApplicationUser> actualApplicationUsers =
                this.applicationUserService.RetrieveAllUsers();

            // then
            actualApplicationUsers.Should().BeEquivalentTo(
                expectedApplicationUsers);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectAllUsers(),
                Times.Once());

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}