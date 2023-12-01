// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Threading.Tasks;
using User.Core.Models.Users;

namespace User.Core.Services.Foundations.Users
{
    public partial interface IApplicationUserService
    {
        ValueTask<ApplicationUser> AddUserAsync(ApplicationUser user, string password);
        ValueTask<ApplicationUser> RetrieveUserByIdAsync(Guid applicationUserId);
    }
}