// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using User.Core.Models.Users;
using User.Core.Models.Users.Exceptions;

namespace User.Core.Services.Foundations.Users
{
    public partial class ApplicationUserService
    {
        private static void ValidateApplicationUserOnAdd(
            ApplicationUser applicationUser, string password)
        {
            ValidateApplicationUserIsNotNull(applicationUser);

            Validate(
                (Rule: IsInvalid(applicationUser.Id), Parameter: nameof(ApplicationUser.Id)),
                (Rule: IsInvalid(applicationUser.FirstName), Parameter: nameof(ApplicationUser.FirstName)),
                (Rule: IsInvalid(applicationUser.LastName), Parameter: nameof(ApplicationUser.LastName)),
                (Rule: IsInvalid(applicationUser.UserName), Parameter: nameof(ApplicationUser.UserName)),
                (Rule: IsInvalid(applicationUser.PhoneNumber), Parameter: nameof(ApplicationUser.PhoneNumber)),
                (Rule: IsInvalid(applicationUser.Email), Parameter: nameof(ApplicationUser.Email)),
                (Rule: IsInvalid(applicationUser.CreatedDate), Parameter: nameof(ApplicationUser.CreatedDate)),
                (Rule: IsInvalid(applicationUser.UpdatedDate), Parameter: nameof(ApplicationUser.UpdatedDate)),
                (Rule: IsInvalid(password), Parameter: nameof(ApplicationUser)),

                (Rule: IsNotSame(
                    firstDate: applicationUser.UpdatedDate,
                    secondDate: applicationUser.CreatedDate,
                    secondDateName: nameof(ApplicationUser.CreatedDate)),
                Parameter: nameof(ApplicationUser.UpdatedDate)));
        }

        private static void ValidateApplicationUserIsNotNull(
            ApplicationUser applicationUser)
        {
            if (applicationUser is null)
            {
                throw new NullApplicationUserException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}."
            };

        private static void Validate(params (dynamic Rule, string Parameter)[]
            validations)
        {
            var invalidApplicationUser =
                new InvalidApplicationUserException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidApplicationUser.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidApplicationUser.ThrowIfContainsErrors();
        }
    }
}