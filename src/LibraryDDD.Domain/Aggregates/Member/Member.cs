using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Domain.Common.Interfaces;
using LibraryDDD.Domain.Events;
using LibraryDDD.Domain.Validation;
using LibraryDDD.Shared.Common;

namespace LibraryDDD.Domain.Aggregates.Member;

public class Member : IAggregateRoot
{
    public MemberId Id { get; private set; }
    internal NonEmptyString Name { get; private set; }
    internal ContactInformation ContactInformation { get; private set; }
    internal DateOfBirth DateOfBirth { get; private set; }
    internal MembershipType MembershipType { get; private set; }
    internal MembershipStatus MembershipStatus { get; private set; }
    internal Maybe<StaffId> StaffId { get; private set; }
    internal Maybe<Address> Address { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Member(
        NonEmptyString name, 
        ContactInformation contactInformation,
        DateOfBirth dateOfBirth,
        MembershipType membershipType,
        Maybe<StaffId> staffId,
        Maybe<Address> address)
    {
        Id = MemberId.Create();
        Name = name;
        ContactInformation = contactInformation;
        DateOfBirth = dateOfBirth;
        MembershipType = membershipType;
        MembershipStatus = MembershipStatus.Active;
        StaffId = staffId;
        Address = address;
    }

    public static Result<Member, MemberError> TryCreate(
        NonEmptyString name, 
        ContactInformation contactInformation,
        DateOfBirth dateOfBirth,
        MembershipType membershipType,
        Maybe<StaffId> staffId,
        Maybe<Address> address)
    {

        var validationResult = Validate(membershipType, staffId);
        if (!validationResult.IsSuccess)
            return Result<Member, MemberError>.Fail(validationResult.Error);

        var member = new Member(
            name, 
            contactInformation, 
            dateOfBirth, 
            membershipType,
            staffId,
            address);
        
        member.AddDomainEvent(new MemberRegistered(member.Id));
        return Result<Member, MemberError>.Ok(member);
    }

    private void AddDomainEvent(IDomainEvent @event) =>
        _domainEvents.Add(@event);

    public void ClearDomainEvents() => _domainEvents.Clear();
    
    private static Result<MemberError> Validate(MembershipType membershipType, Maybe<StaffId> staffId)
    {
        if (IsStaffWithoutStaffId(membershipType, staffId))
            return Result<MemberError>.Fail(MemberError.StaffIdRequiredForStaffMembership);

        if (IsNonStaffWithStaffId(membershipType, staffId))
            return Result<MemberError>.Fail(MemberError.InvalidStaffIdForNonStaffMembership);

        return Result<MemberError>.Ok();
    }
    
    private static bool IsStaffWithoutStaffId(MembershipType membershipType, Maybe<StaffId> staffId) 
        => membershipType == MembershipType.Staff && staffId is Maybe<StaffId>.None;
    
    private static bool IsNonStaffWithStaffId(MembershipType membershipType, Maybe<StaffId> staffId) 
        => membershipType != MembershipType.Staff && staffId is Maybe<StaffId>.Some;
}