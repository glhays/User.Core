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
        private readonly UserManager<ApplicationUser> userManager;

        public UserManagementBroker(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async ValueTask<IdentityResult> InsertUserAsync(ApplicationUser user, string password)
        {
            var broker = new UserManagementBroker(this.userManager);
            return await broker.userManager.CreateAsync(user, password);
        }

        public IQueryable<ApplicationUser> SelectAllUsers() => throw new NotImplementedException();

        public ValueTask<ApplicationUser> SelectUserByIdAsync(Guid userId) =>
            throw new NotImplementedException();

        public ValueTask<ApplicationUser> UpdateUserAsync(ApplicationUser user) =>
            throw new NotImplementedException();

        public ValueTask<ApplicationUser> DeleteUserAsync(ApplicationUser user) =>
            throw new NotImplementedException();

        public ValueTask<ApplicationUser> FindByIdAsync(string id) =>
            throw new NotImplementedException();

        public ValueTask<ApplicationUser> FindByNameAsync(string userName) =>
            throw new NotImplementedException();

        public ValueTask<IList<string>> GetRolesAsync(ApplicationUser user) =>
            throw new NotImplementedException();

        public ValueTask<IdentityResult> AddToRoleAsync(ApplicationUser user, string role) =>
            throw new NotImplementedException();

        public ValueTask<ApplicationUser> FindByEmailAsync(string email) =>
            throw new NotImplementedException();

        public ValueTask<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token) =>
            throw new NotImplementedException();

        public ValueTask<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user) =>
            throw new NotImplementedException();

        public ValueTask<bool> CheckPasswordAsync(ApplicationUser user, string password) =>
            throw new NotImplementedException();

        public ValueTask<string> GeneratePasswordResetTokenAsync(ApplicationUser user) =>
            throw new NotImplementedException();

        public ValueTask<IdentityResult> ResetPasswordAsync(
            ApplicationUser user, string token, string password) =>
            throw new NotImplementedException();

        public ValueTask<string> GenerateTwoFactorTokenAsync(ApplicationUser user) =>
            throw new NotImplementedException();

        public ValueTask<IdentityResult> SetTwoFactorEnabledAsync(
            ApplicationUser user, bool enabled) => throw new NotImplementedException();
    }
}