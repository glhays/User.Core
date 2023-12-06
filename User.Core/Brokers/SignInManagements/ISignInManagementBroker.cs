// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using User.Core.Models.Users;

namespace User.Core.Brokers.SignInManagements
{
    public interface ISignInManagementBroker
    {
        ValueTask SignOutAsync();

        ValueTask<SignInResult> PasswordSignInAsync(
            ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);

        ValueTask<SignInResult> TwoFactorSignInAsync(
            string provider, string code, bool isPersistent, bool rememberClient);
    }
}