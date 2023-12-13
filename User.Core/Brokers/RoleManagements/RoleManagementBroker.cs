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
    public class RoleManagementBroker : IRoleManagementBroker
    {
        public ValueTask<UserRole> InsertRoleAsync(UserRole userRole)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserRole> SelectAllRoles()
        {
            throw new NotImplementedException();
        }

        public ValueTask<UserRole> SelectRoleByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public ValueTask<UserRole> UpdateRoleAsync(UserRole userRole)
        {
            throw new NotImplementedException();
        }

        public ValueTask<UserRole> DeleteRoleAsync(UserRole userRole)
        {
            throw new NotImplementedException();
        }
    }
}