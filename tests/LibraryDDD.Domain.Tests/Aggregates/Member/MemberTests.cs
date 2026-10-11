using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Domain.Events;
using LibraryDDD.Domain.Validation;
using LibraryDDD.Shared.Common;

namespace LibraryDDD.Domain.Tests;

public class MemberTests
{
    [Theory]
    [InlineData("Adult")]
    [InlineData("Child")]
    public void TryCreate_NonStaffWithoutStaffId_CreatesActiveMember(string membershipType)
    {
        var result = CreateMember(
            membershipType,
            new Maybe<StaffId>.None());

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id.Value);
        Assert.Equal(MembershipStatus.Active, result.Value.MembershipStatus);
        Assert.IsType<Maybe<StaffId>.None>(result.Value.StaffId);
        Assert.Single(result.Value.DomainEvents);
        var registeredEvent = Assert.IsType<MemberRegistered>(result.Value.DomainEvents.Single());
        Assert.Equal(result.Value.Id, registeredEvent.MemberId);
    }

    [Fact]
    public void TryCreate_StaffWithStaffId_CreatesMember()
    {
        var staffId = StaffId.TryCreate("STAFF-123").Value;

        var result = CreateMember(
            "Staff",
            new Maybe<StaffId>.Some(staffId));

        Assert.True(result.IsSuccess);
        Assert.Equal(staffId, Assert.IsType<Maybe<StaffId>.Some>(result.Value.StaffId).Value);
    }

    [Fact]
    public void TryCreate_StaffWithoutStaffId_ReturnsFailure()
    {
        var result = CreateMember(
            "Staff",
            new Maybe<StaffId>.None());

        Assert.False(result.IsSuccess);
        Assert.Equal(MemberError.StaffIdRequiredForStaffMembership, result.Error);
    }

    [Theory]
    [InlineData("Adult")]
    [InlineData("Child")]
    public void TryCreate_NonStaffWithStaffId_ReturnsFailure(string membershipType)
    {
        var staffId = StaffId.TryCreate("STAFF-123").Value;

        var result = CreateMember(
            membershipType,
            new Maybe<StaffId>.Some(staffId));

        Assert.False(result.IsSuccess);
        Assert.Equal(MemberError.InvalidStaffIdForNonStaffMembership, result.Error);
    }

    [Fact]
    public void ClearDomainEvents_RemovesPendingEvents()
    {
        var result = CreateMember(
            "Adult",
            new Maybe<StaffId>.None());

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.DomainEvents);

        result.Value.ClearDomainEvents();

        Assert.Empty(result.Value.DomainEvents);
    }

    private static Result<Member, MemberError> CreateMember(
        string membershipType,
        Maybe<StaffId> staffId)
    {
        var name = NonEmptyString.TryCreate("Alex Example").Value;
        var email = Email.TryCreate("alex@example.com").Value;
        var contactInformation = ContactInformation.TryCreate(
            new Maybe<Email>.Some(email),
            new Maybe<PhoneNo>.None()).Value;
        var dateOfBirth = DateOfBirth.TryCreate(new DateOnly(1990, 1, 1)).Value;
        var type = MembershipType.TryCreate(membershipType).Value;

        return Member.TryCreate(
            name,
            contactInformation,
            dateOfBirth,
            type,
            staffId,
            new Maybe<Address>.None());
    }
}
