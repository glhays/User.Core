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
    public partial interface IStorageBroker
    {
        ValueTask<Member> InsertMemberAsync(Member member);
        IQueryable<Member> SelectAllMembersAsync();
        ValueTask<Member> SelectMemberByIdAsync(Guid id);
        ValueTask<Member> UpdateInternAsync(Member member);
        ValueTask<Member> DeleteMemberAsync(Member member);
    }
}