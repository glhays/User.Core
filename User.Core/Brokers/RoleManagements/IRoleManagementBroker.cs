// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using User.Core.Models.Roles;

namespace User.Core.Brokers.RoleManagements
{
    public interface IRoleManagementBroker
    {
        ValueTask<ApplicationRole> InsertRoleAsync(ApplicationRole user);
        IQueryable<ApplicationRole> SelectAllRoles();
        ValueTask<ApplicationRole> SelectRoleByIdAsync(Guid userId);
        ValueTask<ApplicationRole> UpdateRoleAsync(ApplicationRole user);
        ValueTask<ApplicationRole> DeleteRoleAsync(ApplicationRole user);
    }
}