// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using User.Core.Models.Users;

namespace User.Core.Brokers.UserManagements
{
    public class UserManagementBroker : IUserManagementBroker
    {
        private readonly UserManager<ApplicationUser> userManagement;

        public UserManagementBroker(UserManager<ApplicationUser> userManager) =>
            this.userManagement = userManager;

        public async ValueTask<IdentityResult> InsertUserAsync(ApplicationUser user, string password)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.CreateAsync(user, password);
        }

        public IQueryable<ApplicationUser> SelectAllUsers() => this.userManagement.Users;

        public async ValueTask<ApplicationUser> SelectUserByIdAsync(Guid userId)
        {
            var broker = new UserManagementBroker(this.userManagement);

            return await broker.userManagement.FindByIdAsync(userId.ToString());
        }

        public async ValueTask<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            var broker = new UserManagementBroker(this.userManagement);
            await broker.userManagement.UpdateAsync(user);

            return user;
        }

        public async ValueTask<ApplicationUser> DeleteUserAsync(ApplicationUser user)
        {
            var broker = new UserManagementBroker(this.userManagement);
            await broker.userManagement.DeleteAsync(user);

            return user;
        }

        public async ValueTask<IdentityResult> UpdateUserPasswordAsync(
            ApplicationUser user, string token, string password)
        {
            var broker = new UserManagementBroker(this.userManagement);
            
            return await broker.userManagement.ResetPasswordAsync(user, token, password);
        }
            

        public ValueTask<ApplicationUser> FindByIdAsync(string id) =>
            throw new NotImplementedException();

        public ValueTask<ApplicationUser> FindByNameAsync(string userName) =>
            throw new NotImplementedException();

        public ValueTask<ApplicationUser> FindByEmailAsync(string email) =>
            throw new NotImplementedException();

        public ValueTask<IList<string>> GetRolesAsync(ApplicationUser user) =>
            throw new NotImplementedException();

        public ValueTask<IdentityResult> AddToRoleAsync(ApplicationUser user, string role) =>
            throw new NotImplementedException();

        public ValueTask<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token) =>
            throw new NotImplementedException();

        public ValueTask<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user) =>
            throw new NotImplementedException();

        public ValueTask<bool> CheckPasswordAsync(ApplicationUser user, string password) =>
            throw new NotImplementedException();

        public ValueTask<string> GeneratePasswordResetTokenAsync(ApplicationUser user) =>
            throw new NotImplementedException();


        public ValueTask<string> GenerateTwoFactorTokenAsync(ApplicationUser user) =>
            throw new NotImplementedException();

        public ValueTask<IdentityResult> SetTwoFactorEnabledAsync(
            ApplicationUser user, bool enabled) => throw new NotImplementedException();
    }
}