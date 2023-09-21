// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace User.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string HomeRelativeUrl = "api/home";
        
        public async ValueTask<string> GetHomeMessage() =>
            await this.apiFactoryClient.GetContentStringAsync(HomeRelativeUrl);
    }
}