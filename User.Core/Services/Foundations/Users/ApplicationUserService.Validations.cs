// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using User.Core.Models.Users;
using User.Core.Models.Users.Exceptions;

namespace User.Core.Services.Foundations.Users
{
    public partial class ApplicationUserService
    {
        private static void ValidateApplicationUserOnAdd(
            ApplicationUser applicationUser)
        {
            ValidateApplicationUserIsNotNull(applicationUser);
        }

        private static void ValidateApplicationUserIsNotNull(
            ApplicationUser applicationUser)
        {
            if (applicationUser is null)
            {
                throw new NullApplicationUserException();
            }
        }
    }
}