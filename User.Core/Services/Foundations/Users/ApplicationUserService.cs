﻿// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Linq;
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

        public ValueTask<ApplicationUser> AddUserAsync(ApplicationUser user, string password) =>
        TryCatch(async () =>
        {
            ValidateApplicationUserOnAdd(user, password);
            await this.userManagementBroker.InsertUserAsync(user, password);

            return user;
        });

        public ValueTask<ApplicationUser> RetrieveUserByIdAsync(Guid applicationUserId) =>
        TryCatch(async () =>
        {
            ValidateApplicationUserId(applicationUserId);

            ApplicationUser maybeApplicationUser =
                await this.userManagementBroker.SelectUserByIdAsync(applicationUserId);

            ValidateStorageApplicationUser(maybeApplicationUser, applicationUserId);

            return maybeApplicationUser;
        });

        public IQueryable<ApplicationUser> RetrieveAllUsers() =>
        TryCatch(() => this.userManagementBroker.SelectAllUsers());

        public ValueTask<ApplicationUser> ModifyUserAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateApplicationUserOnModify(user);

            ApplicationUser maybeApplicationUser =
                await this.userManagementBroker.SelectUserByIdAsync(user.Id);

            ValidateStorageApplicationUser(maybeApplicationUser, user.Id);

            ValidateAgainstStorageApplicationUserOnModify(
                inputApplicationUser: user,
                storageApplicationUser: maybeApplicationUser);

            return await this.userManagementBroker.UpdateUserAsync(user);
        });

        public ValueTask<ApplicationUser> RemoveUserByIdAsync(Guid applicationUserId) =>
        TryCatch(async () =>
        {
            ValidateApplicationUserId(applicationUserId);

            ApplicationUser maybeApplicationUser =
                await this.userManagementBroker.SelectUserByIdAsync(applicationUserId);

            ValidateStorageApplicationUser(maybeApplicationUser, applicationUserId);

            return await this.userManagementBroker.DeleteUserAsync(maybeApplicationUser);
        });

        public ValueTask<ApplicationUser> ModifyUserPasswordAsync(
            ApplicationUser user, string token, string password) =>
        TryCatch(async () =>
        {
            ValidateOnModifyPassword(user, token, password);

            var maybeResult =
                await this.userManagementBroker.UpdateUserPasswordAsync(user, token, password);

            return user;
        });

        public ValueTask<string> RetrieveUserPasswordResetTokenAsync(ApplicationUser user) =>
        TryCatch(async () =>
        {
            ValidateApplicationUser(user);
            
            return await this.userManagementBroker.SelectUserPasswordResetTokenAsync(user);
        });
    }
}