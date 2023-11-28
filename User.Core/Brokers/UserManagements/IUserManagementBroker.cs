// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using User.Core.Models.Members;

namespace User.Core.Brokers.UserManagements
{
    public interface IUserManagementBroker
    {
        ValueTask<IdentityResult> InsertUserAsync(ApplicationUser user, string password);
        IQueryable<ApplicationUser> SelectAllUsers();
        ValueTask<ApplicationUser> SelectUserByIdAsync(Guid userId);
        ValueTask<ApplicationUser> UpdateUserAsync(ApplicationUser user);
        ValueTask<ApplicationUser> DeleteUserAsync(ApplicationUser user);

        ValueTask<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        ValueTask<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        ValueTask<string> GenerateTwoFactorTokenAsync(ApplicationUser user);
        ValueTask<ApplicationUser> FindByNameAsync(string userName);
        ValueTask<ApplicationUser> FindByEmailAsync(string email);
        ValueTask<bool> CheckPasswordAsync(ApplicationUser user, string password);
        ValueTask<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string password);
        ValueTask<IdentityResult> SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled);
        ValueTask<IList<string>> GetRolesAsync(ApplicationUser user);
        ValueTask<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        ValueTask<ApplicationUser> FindByIdAsync(string id);
        ValueTask<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
    }
}