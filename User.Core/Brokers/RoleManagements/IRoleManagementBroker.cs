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
        ValueTask<UserRole> InsertRoleAsync(UserRole userRole);
        IQueryable<UserRole> SelectAllRoles();
        ValueTask<UserRole> SelectRoleByIdAsync(Guid userId);
        ValueTask<UserRole> UpdateRoleAsync(UserRole userRole);
        ValueTask<UserRole> DeleteRoleAsync(UserRole userRole);
    }
}