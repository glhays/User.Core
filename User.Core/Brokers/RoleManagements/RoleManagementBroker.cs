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
        public ValueTask<ApplicationRole> InsertRoleAsync(ApplicationRole user)
        {
            throw new NotImplementedException();
        }
     
        public IQueryable<ApplicationRole> SelectAllRoles()
        {
            throw new NotImplementedException();
        }
        
        public ValueTask<ApplicationRole> SelectRoleByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ApplicationRole> UpdateRoleAsync(ApplicationRole user)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ApplicationRole> DeleteRoleAsync(ApplicationRole user)
        {
            throw new NotImplementedException();
        }
    }
}