// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System.Threading.Tasks;
using User.Core.Brokers.DateTimes;
using User.Core.Brokers.Loggings;
using User.Core.Brokers.UserManagements;
using User.Core.Models.Users;

namespace User.Core.Services.Foundations.Users
{
    public partial class ApplicationUserService : IApplicationUserService
    {
        private readonly IUserManagementBroker userManagementBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ApplicationUserService(
            IUserManagementBroker userManagementBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.userManagementBroker = userManagementBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ApplicationUser> AddUserAsync(ApplicationUser user, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}