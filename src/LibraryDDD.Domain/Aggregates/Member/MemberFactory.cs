using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Domain.Validation;
using LibraryDDD.Shared.Common;
namespace LibraryDDD.Domain.Aggregates.Member;

public static class MemberFactory
{
    public static Result<Member, MemberError> TryCreate(
        NonEmptyString name,
        ContactInformation contactInformation,
        DateOnly dateOfBirth,
        MembershipType membershipType,
        Maybe<StaffId> staffId,
        Maybe<Address> address
    )
    {
        return Member.TryCreate(
            name,
            contactInformation,
            dateOfBirth,
            membershipType,
            staffId,
            address
        );
    }
}