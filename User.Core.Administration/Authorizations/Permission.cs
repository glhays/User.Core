// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

namespace User.Core.Administration.Authorizations
{
    public enum Permission
    {
        None = 0,
        ViewRoles = 1,
        ManageRoles = 2,
        ViewUsers = 4,
        ManageUsers = 8,
        ConfigureAccessControl = 16,
        InitiateRefund = 32,
        SplitPayments = 64,
        ViewAccessControl = 128,
        All = ~None
    }
}