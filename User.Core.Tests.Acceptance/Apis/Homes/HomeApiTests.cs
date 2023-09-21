// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using User.Core.Tests.Acceptance.Brokers;
using Xunit;

namespace User.Core.Tests.Acceptance.Apis.Homes
{
    [Collection(nameof(ApiTestCollection))]
    public partial class HomeApiTests
    {
        private readonly ApiBroker apiBroker;

        public HomeApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        [Fact]
        private async Task ShouldReturnHomeMessageAsync()
        {
            // given
            string expectedMessage =
                "The core endpoint Home has been reached.";

            // when
            string actualMessage =
                await this.apiBroker.GetHomeMessage();

            // then
            actualMessage.Should().Be(expectedMessage);
        }
    }
}