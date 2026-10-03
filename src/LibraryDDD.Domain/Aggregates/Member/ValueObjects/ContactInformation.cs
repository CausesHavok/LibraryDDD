using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Domain.Validation;
using LibraryDDD.Shared.Common;
namespace LibraryDDD.Domain.Aggregates.Member;

internal record ContactInformation
{
    public Maybe<Email> Email { get; }
    public Maybe<PhoneNo> PhoneNo { get; }

    private ContactInformation(Maybe<Email> email, Maybe<PhoneNo> phoneNo)
    {
        Email = email;
        PhoneNo = phoneNo;
    }

    public static Result<ContactInformation, ContactInformationError> TryCreate(Maybe<Email> email, Maybe<PhoneNo> phoneNo)
    {
        if (!IsContactInformationValid(email, phoneNo))
            return Result<ContactInformation, ContactInformationError>.Fail(ContactInformationError.MissingEmailAndPhoneNo);

        return Result<ContactInformation, ContactInformationError>.Ok(new ContactInformation(email, phoneNo));
    }

    private static bool IsContactInformationValid(Maybe<Email> email, Maybe<PhoneNo> phoneNo) =>
        email is Maybe<Email>.Some || phoneNo is Maybe<PhoneNo>.Some;
}