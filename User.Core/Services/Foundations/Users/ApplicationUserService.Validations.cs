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
        private void ValidateApplicationUserOnAdd(
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
                Parameter: nameof(ApplicationUser.UpdatedDate)),

                (Rule: IsNotRecent(applicationUser.CreatedDate),
                    Parameter: nameof(ApplicationUser.CreatedDate)));
        }

        private void ValidateApplicationUserOnModify(ApplicationUser applicationUser)
        {
            ValidateApplicationUserIsNotNull(applicationUser);

            Validate(
                (Rule: IsInvalid(applicationUser.FirstName), Parameter: nameof(ApplicationUser.FirstName)),
                (Rule: IsInvalid(applicationUser.LastName), Parameter: nameof(ApplicationUser.LastName)),
                (Rule: IsInvalid(applicationUser.UserName), Parameter: nameof(ApplicationUser.UserName)),
                (Rule: IsInvalid(applicationUser.PhoneNumber), Parameter: nameof(ApplicationUser.PhoneNumber)),
                (Rule: IsInvalid(applicationUser.Email), Parameter: nameof(ApplicationUser.Email)),
                (Rule: IsInvalid(applicationUser.CreatedDate), Parameter: nameof(ApplicationUser.CreatedDate)),
                (Rule: IsInvalid(applicationUser.UpdatedDate), Parameter: nameof(ApplicationUser.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: applicationUser.UpdatedDate,
                    secondDate: applicationUser.CreatedDate,
                    secondDateName: nameof(ApplicationUser.CreatedDate)),
                Parameter: nameof(ApplicationUser.UpdatedDate)),

                (Rule: IsNotRecent(applicationUser.UpdatedDate),
                    Parameter: nameof(ApplicationUser.UpdatedDate)));
        }

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDateTime =
                this.dateTimeBroker.GetCurrentDateTimeOffset();

            TimeSpan timeDifference = currentDateTime.Subtract(date);
            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            return timeDifference.Duration() > oneMinute;
        }

        private static void ValidateApplicationUserIsNotNull(
            ApplicationUser applicationUser)
        {
            if (applicationUser is null)
            {
                throw new NullApplicationUserException();
            }
        }

        private static void ValidateApplicationUserId(Guid id) =>
            Validate((Rule: IsInvalid(id), Parameter: nameof(ApplicationUser.Id)));

        private static void ValidateStorageApplicationUser(
            ApplicationUser maybeApplicationUser, Guid id)
        {
            if (maybeApplicationUser is null)
            {
                throw new NotFoundApplicationUserException(id);
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

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
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