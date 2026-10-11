using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Shared.Common;

namespace LibraryDDD.Infrastructure.Tests.TestSupport;

internal static class MemberTestFactory
{
    public static Member CreateMember()
    {
        var name = NonEmptyString.TryCreate("Alex Example").Value;
        var email = Email.TryCreate("alex@example.com").Value;
        var contactInformation = ContactInformation.TryCreate(
            new Maybe<Email>.Some(email),
            new Maybe<PhoneNo>.None()).Value;
        var dateOfBirth = DateOfBirth.TryCreate(new DateOnly(1990, 1, 1)).Value;
        var membershipType = MembershipType.TryCreate("Adult").Value;

        return Member.TryCreate(
            name,
            contactInformation,
            dateOfBirth,
            membershipType,
            new Maybe<StaffId>.None(),
            new Maybe<Address>.None()).Value;
    }
}
