using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Domain.Common.Interfaces;
using LibraryDDD.Domain.Events;

namespace LibraryDDD.Domain.Aggregates.Member;

public class Member : IAggregateRoot
{
    public MemberId Id { get; private set; }
    internal NonEmptyString Name { get; private set; }
    internal ContactInformation ContactInformation { get; private set; }
    internal DateOnly DateOfBirth { get; private set; }
    internal MembershipType MembershipType { get; private set; }
    internal MembershipStatus MembershipStatus { get; private set; }
    internal Maybe<StaffId> StaffId { get; private set; }
    internal Maybe<Address> Address { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Member(
        NonEmptyString name, 
        ContactInformation contactInformation,
        DateOnly dateOfBirth,
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

        Validate();
    }

    internal static Member Create(
        NonEmptyString name, 
        ContactInformation contactInformation,
        DateOnly dateOfBirth,
        MembershipType membershipType,
        Maybe<StaffId> staffId,
        Maybe<Address> address)
    {
        var member = new Member(
            name, 
            contactInformation, 
            dateOfBirth, 
            membershipType,
            staffId,
            address);
        member.AddDomainEvent(new MemberRegistered(member.Id));
        return member;
    }

    private void AddDomainEvent(IDomainEvent @event) =>
        _domainEvents.Add(@event);

    public void ClearDomainEvents() => _domainEvents.Clear();
    
    private void Validate()
    {
        if (IsStaffWithoutStaffId())
            throw new InvalidOperationException("Staff members must have a valid Staff ID.");

        if (IsNonStaffWithStaffId())
            throw new InvalidOperationException("Only staff members can have a Staff ID.");
    }
    
    private bool IsStaffWithoutStaffId() => MembershipType == MembershipType.Staff && StaffId is Maybe<StaffId>.None;
    
    private bool IsNonStaffWithStaffId() => MembershipType != MembershipType.Staff && StaffId is Maybe<StaffId>.Some;
}