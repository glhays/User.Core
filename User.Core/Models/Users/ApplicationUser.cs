// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using Microsoft.AspNetCore.Identity;
using User.Core.Models.Users;

namespace User.Core.Models.Members
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public override Guid Id
        {
            get => base.Id;
            set => base.Id = value;
        }

        public override string UserName
        {
            get => base.UserName;
            set => base.UserName = value;
        }
        public override string PhoneNumber
        {
            get => base.PhoneNumber;
            set => base.PhoneNumber = value;
        }
        public override string Email
        {
            get => base.Email;
            set => base.Email = value;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public UserStatus Status { get; set; }
    }
}