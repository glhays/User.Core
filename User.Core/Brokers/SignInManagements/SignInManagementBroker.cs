// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using User.Core.Models.Users;

namespace User.Core.Brokers.SignInManagements
{
    public class SignInManagementBroker : ISignInManagementBroker
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        public ValueTask<SignInResult> PasswordSignInAsync(
            ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            throw new System.NotImplementedException();
        }
        public ValueTask<SignInResult> TwoFactorSignInAsync(
            string provider, string code, bool isPersistent, bool rememberClient)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask SignOutAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}