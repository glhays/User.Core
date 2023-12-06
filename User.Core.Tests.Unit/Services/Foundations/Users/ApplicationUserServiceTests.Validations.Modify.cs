// -----------------------------------------------------------
// Copyright(c) Coalition of the Good-Hearted Engineers
// ======= FREE TO USE FOR THE WORLD =======
// -----------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using User.Core.Models.Users;
using User.Core.Models.Users.Exceptions;
using Xeptions;
using Xunit;

namespace User.Core.Tests.Unit.Services.Foundations.Users
{
    public partial class ApplicationUserServiceTests
    {
        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfApplicationUserIsNullAndLogItAsync()
        {
            // given
            ApplicationUser nullApplicationUser = null;
            var innerException = new Exception();

            var nullApplicationUserException =
                new NullApplicationUserException(
                    message: "ApplicationUser is null, please fix and try again.",
                    innerException: innerException);

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: nullApplicationUserException);

            // when
            ValueTask<ApplicationUser> modifyApplicationUserTask =
                this.applicationUserService.ModifyUserAsync(nullApplicationUser);

            ApplicationUserValidationException actualApplicationUserValidationException =
                await Assert.ThrowsAsync<ApplicationUserValidationException>(
                    modifyApplicationUserTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<ApplicationUser>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        private async Task ShouldThrowValidationExceptionOnModifyIfApplicationUserIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidApplicationUser = new ApplicationUser
            {
                FirstName = invalidText,
                LastName = invalidText
            };

            var invalidApplicationUserException =
                new InvalidApplicationUserException(
                    message: "Invalid ApplicationUser, correct errors to continue.");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.FirstName),
                values: "Text is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.LastName),
                values: "Text is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.UserName),
                values: "Text is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.PhoneNumber),
                values: "Text is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.Email),
                values: "Text is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.CreatedDate),
                values: "Date is required");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.UpdatedDate),
                values: new[]
                {
                    "Date is required",
                    $"Date is the same as {nameof(ApplicationUser.CreatedDate)}."
                });

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: invalidApplicationUserException);

            // when
            ValueTask<ApplicationUser> modifyApplicationUserTask =
                this.applicationUserService.ModifyUserAsync(invalidApplicationUser);

            ApplicationUserValidationException actualApplicationUserValidationException =
               await Assert.ThrowsAsync<ApplicationUserValidationException>(
                   modifyApplicationUserTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeoffset = GetRandomDateTimeOffset();

            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser(randomDateTimeoffset);

            ApplicationUser invalidApplicationUser = randomApplicationUser;

            var invalidApplicationUserException =
                new InvalidApplicationUserException(
                    message: "Invalid ApplicationUser, correct errors to continue.");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.UpdatedDate),
                values: $"Date is the same as {nameof(ApplicationUser.CreatedDate)}.");

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: invalidApplicationUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeoffset);

            // when
            ValueTask<ApplicationUser> modifyApplicationUserTask =
                this.applicationUserService.ModifyUserAsync(invalidApplicationUser);

            ApplicationUserValidationException actualApplicationUserValidationException =
               await Assert.ThrowsAsync<ApplicationUserValidationException>(
                   modifyApplicationUserTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(invalidApplicationUser.Id),
                Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        private async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser(randomDateTimeOffset);

            randomApplicationUser.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidApplicationUserException =
                new InvalidApplicationUserException(
                    message: "Invalid ApplicationUser, correct errors to continue.");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.UpdatedDate),
                values: $"Date is not recent");

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: invalidApplicationUserException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<ApplicationUser> modifyApplicationUserTask =
                this.applicationUserService.ModifyUserAsync(randomApplicationUser);

            ApplicationUserValidationException actualApplicationUserValidationException =
               await Assert.ThrowsAsync<ApplicationUserValidationException>(
                   modifyApplicationUserTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagementBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfApplicationUserDoesNotExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser(randomDateTimeOffset);

            ApplicationUser nonExistentApplicationUser = randomApplicationUser;

            nonExistentApplicationUser.CreatedDate =
                randomDateTimeOffset.AddMinutes(randomNegativeMinutes);

            ApplicationUser nullApplicationUser = null;
            var innerException = new Exception();

            var notFoundApplicationUserException =
                new NotFoundApplicationUserException(
                    message: $"ApplicationUser not found with id: {nonExistentApplicationUser.Id}.",
                    innerException: innerException.As<Xeption>());

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: notFoundApplicationUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(nonExistentApplicationUser.Id))
                .ReturnsAsync(nullApplicationUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<ApplicationUser> modifyApplicationUserTask =
                this.applicationUserService.ModifyUserAsync(
                    nonExistentApplicationUser);

            ApplicationUserValidationException actualApplicationUserValidationException =
               await Assert.ThrowsAsync<ApplicationUserValidationException>(
                   modifyApplicationUserTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(nonExistentApplicationUser.Id),
                Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            ApplicationUser randomApplicationUser =
                CreateRandomApplicationUser(randomDateTimeOffset);

            ApplicationUser invalidApplicationUser = randomApplicationUser.DeepClone();
            ApplicationUser storageApplicationUser = randomApplicationUser.DeepClone();

            storageApplicationUser.CreatedDate =
                storageApplicationUser.CreatedDate.AddMinutes(randomMinutes);

            storageApplicationUser.UpdatedDate =
                storageApplicationUser.UpdatedDate.AddMinutes(randomMinutes);

            Guid applicationUserId = invalidApplicationUser.Id;

            invalidApplicationUser.CreatedDate =
                storageApplicationUser.CreatedDate.AddMinutes(randomMinutes);

            var invalidApplicationUserException =
                new InvalidApplicationUserException(
                    message: "Invalid ApplicationUser, correct errors to continue.");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.CreatedDate),
                values: $"Date is not the same as {nameof(ApplicationUser.CreatedDate)}.");

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: invalidApplicationUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(applicationUserId))
                .ReturnsAsync(storageApplicationUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<ApplicationUser> modifyApplicationUserTask =
                this.applicationUserService.ModifyUserAsync(
                    invalidApplicationUser);

            ApplicationUserValidationException actualApplicationUserValidationException =
               await Assert.ThrowsAsync<ApplicationUserValidationException>(
                   modifyApplicationUserTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(invalidApplicationUser.Id),
                Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateIsSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            ApplicationUser randomApplicationUser =
                CreateRandomModifyApplicationUser(randomDateTimeOffset);

            ApplicationUser invalidApplicationUser = randomApplicationUser;
            ApplicationUser storageApplicationUser = randomApplicationUser.DeepClone();
            invalidApplicationUser.UpdatedDate = storageApplicationUser.UpdatedDate;

            var invalidApplicationUserException =
                new InvalidApplicationUserException(
                    message: "Invalid ApplicationUser, correct errors to continue.");

            invalidApplicationUserException.AddData(
                key: nameof(ApplicationUser.UpdatedDate),
                values: $"Date is the same as {nameof(ApplicationUser.UpdatedDate)}.");

            var expectedApplicationUserValidationException =
                new ApplicationUserValidationException(
                    message: "ApplicationUser validation errors occurred, please try again.",
                    innerException: invalidApplicationUserException);

            this.userManagementBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(invalidApplicationUser.Id))
                .ReturnsAsync(storageApplicationUser);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<ApplicationUser> modifyApplicationUserTask =
                this.applicationUserService.ModifyUserAsync(
                    invalidApplicationUser);

            ApplicationUserValidationException actualApplicationUserValidationException =
               await Assert.ThrowsAsync<ApplicationUserValidationException>(
                   modifyApplicationUserTask.AsTask);

            // then
            actualApplicationUserValidationException.Should().BeEquivalentTo(
                expectedApplicationUserValidationException);

            this.userManagementBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(invalidApplicationUser.Id),
                Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedApplicationUserValidationException))),
                    Times.Once);

            this.userManagementBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}