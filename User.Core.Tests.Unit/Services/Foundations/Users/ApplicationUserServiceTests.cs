// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using Moq;
using Tynamix.ObjectFiller;
using User.Core.Brokers.DateTimes;
using User.Core.Brokers.Loggings;
using User.Core.Brokers.UserManagements;
using User.Core.Models.Users;
using User.Core.Services.Foundations.Users;
using Xeptions;
using Xunit;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        private readonly Mock<IUserManagementBroker> userManagementBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IApplicationUserService applicationUserService;

        public ApplicationUserServiceTests()
        {
            this.userManagementBrokerMock = new Mock<IUserManagementBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.applicationUserService = new ApplicationUserService(
                this.userManagementBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.loggingBrokerMock.Object);
        }

        private static ApplicationUser CreateRandomApplicationUser() =>
            GenerateApplicationUser();
        
        private static ApplicationUser CreateRandomApplicationUser(DateTimeOffset date) =>
            GenerateApplicationUser(date);

        private static ApplicationUser GenerateApplicationUser()
        {
            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = new EmailAddresses().GetValue(),
                FirstName = new RealNames(NameStyle.FirstName).GetValue(),
                LastName = new RealNames(NameStyle.LastName).GetValue(),
                UserName = new MnemonicString(wordCount: 1, 8, 15).GetValue(),
                PhoneNumber = GetRandomPhoneNumber().ToString(),
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedDate = DateTimeOffset.UtcNow
            };

                return applicationUser;
        }
        
        private static ApplicationUser GenerateApplicationUser(DateTimeOffset dates)
        {
            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = new EmailAddresses().GetValue(),
                FirstName = new RealNames(NameStyle.FirstName).GetValue(),
                LastName = new RealNames(NameStyle.LastName).GetValue(),
                UserName = new MnemonicString(wordCount: 1, 8, 15).GetValue(),
                PhoneNumber = GetRandomPhoneNumber().ToString(),
                CreatedDate = dates,
                UpdatedDate = dates
            };

                return applicationUser;
        }

        private static object GetRandomPhoneNumber()
        {
            string plusCode = "+1";
            
            var randomPhoneNumber = 
               new string(Enumerable.Range(1, 10)
               .Select(i => $"{RandomNumberGenerator.GetInt32(0, 10)}"[0]).ToArray());
            
            var phoneNumber = $"{plusCode}+{randomPhoneNumber}";

            return phoneNumber;
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();
        
        private static string GetRandomPassword() =>
            new MnemonicString(wordCount: 1, wordMinLength: 8, wordMaxLength: 20).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        public static TheoryData MinutesBeforeOrAfter()
        {
            int randomNumber = GetRandomNumber();
            int randomNegativeNumber = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                randomNumber,
                randomNegativeNumber
            };
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
               actualException => actualException.SameExceptionAs(expectedException);

        private static Filler<ApplicationUser> CreateApplicationUserFiller(DateTimeOffset dates)
        {
            var filler = new Filler<ApplicationUser>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}