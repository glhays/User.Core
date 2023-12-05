// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using User.Core.Models.Users;

namespace User.Core.Services.Foundations.Users
{
    public partial interface IApplicationUserService
    {
        ValueTask<ApplicationUser> AddUserAsync(ApplicationUser user, string password);
        ValueTask<ApplicationUser> RetrieveUserByIdAsync(Guid applicationUserId);
        IQueryable<ApplicationUser> RetrieveAllUsers();
        ValueTask<ApplicationUser> ModifyUserAsync(ApplicationUser user);
        ValueTask<ApplicationUser> RemoveUserByIdAsync(Guid applicationUserId);
    }
}