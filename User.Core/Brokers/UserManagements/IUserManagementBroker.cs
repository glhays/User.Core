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
    public interface IUserManagementBroker
    {
        ValueTask<IdentityResult> InsertUserAsync(ApplicationUser user, string password);
        IQueryable<ApplicationUser> SelectAllUsers();
        ValueTask<ApplicationUser> SelectUserByIdAsync(Guid userId);
        ValueTask<ApplicationUser> UpdateUserAsync(ApplicationUser user);
        ValueTask<ApplicationUser> DeleteUserAsync(ApplicationUser user);

        ValueTask<ApplicationUser> FindByIdAsync(string id);
        ValueTask<ApplicationUser> FindByNameAsync(string userName);
        ValueTask<ApplicationUser> FindByEmailAsync(string email);
        ValueTask<IList<string>> GetRolesAsync(ApplicationUser user);
        ValueTask<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        ValueTask<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
        ValueTask<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        ValueTask<bool> CheckPasswordAsync(ApplicationUser user, string password);
        ValueTask<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        ValueTask<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string password);
        ValueTask<string> GenerateTwoFactorTokenAsync(ApplicationUser user);
        ValueTask<IdentityResult> SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled);
    }
}