using LibraryDDD.Domain.Common.DTO;
using LibraryDDD.Domain.Common.ValueObjects;
namespace LibraryDDD.Domain.Aggregates.Member;

public static class MemberFactory
{
    public static Member Create(
        string name,
        string? phoneNumber,
        string? email,
        DateOnly dateOfBirth,
        string membershipType,
        string? staffId,
        AddressFields addressFields
    )
    {
        NonEmptyString _name = new(name);
        ContactInformation contactInformation = CreateContactInformation(email, phoneNumber);
        MembershipType _membershipType = MembershipType.FromString(membershipType);
        Maybe<StaffId> _staffId = ToMaybeStaffId(staffId);
        Maybe<Address> _address = CreateOptionalAddress(addressFields);

        return Member.Create(
            _name,
            contactInformation,
            dateOfBirth,
            _membershipType,
            _staffId,
            _address
        );
    }


    private static Maybe<Address> CreateOptionalAddress(AddressFields fields)
    {
        if (fields.Street is null &&
            fields.City is null &&
            fields.PostalCode is null &&
            fields.Country is null)
            return new Maybe<Address>.None();

        if (fields.Street is null ||
            fields.City is null ||
            fields.PostalCode is null ||
            fields.Country is null)
            throw new ArgumentException("Address is incomplete.");

        return new Maybe<Address>.Some(
            new Address(
                new NonEmptyString(fields.Street),
                new NonEmptyString(fields.City),
                new NonEmptyString(fields.PostalCode),
                new NonEmptyString(fields.Country)
            )
        );
    }

    private static ContactInformation CreateContactInformation(string? email, string? phoneNumber)
    {
        return new ContactInformation(
            ToMaybeEmail(email),
            ToMaybePhoneNo(phoneNumber)
        );
    }

    private static Maybe<Email> ToMaybeEmail(string? email) => 
        email is null 
            ?  new Maybe<Email>.None()
            : new Maybe<Email>.Some(new Email(email));
    
    private static Maybe<PhoneNo> ToMaybePhoneNo(string? phoneNumber) =>
        phoneNumber is null
            ? new Maybe<PhoneNo>.None()
            : new Maybe<PhoneNo>.Some(new PhoneNo(phoneNumber));

    private static Maybe<StaffId> ToMaybeStaffId(string? staffId) =>
        staffId is null
            ? new Maybe<StaffId>.None()
            : new Maybe<StaffId>.Some(new StaffId(staffId));
}