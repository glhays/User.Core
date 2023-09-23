// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using User.Core.Models.Members;

namespace User.Core.Brokers.Storages
{
    public partial class StorageBroker : IStorageBroker
    {
        public ValueTask<Member> InsertMemberAsync(Member member) =>
            throw new NotImplementedException();

        public IQueryable<Member> SelectAllMembersAsync() =>
            throw new NotImplementedException();

        public ValueTask<Member> SelectMemberByIdAsync(Guid id) =>
            throw new NotImplementedException();

        public ValueTask<Member> UpdateInternAsync(Member member) =>
            throw new NotImplementedException();

        public ValueTask<Member> DeleteMemberAsync(Member member) =>
            throw new NotImplementedException();
    }
}