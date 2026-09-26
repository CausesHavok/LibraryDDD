using LibraryDDD.Domain.Common.ValueObjects;
namespace LibraryDDD.Domain.Aggregates.Member;

internal record ContactInformation
{
    public readonly Maybe<Email> Email;
    public readonly Maybe<PhoneNo> PhoneNo;

    public ContactInformation(Maybe<Email> email, Maybe<PhoneNo> phoneNo)
    {
        if (!IsContactInformationValid(email, phoneNo))
            throw new ArgumentException("Contact information must contain at least one of 'Phone number' and/or 'Email Address'");

        Email = email;
        PhoneNo = phoneNo;
    }

    private static bool IsContactInformationValid(Maybe<Email> email, Maybe<PhoneNo> phoneNo) =>
        email is Maybe<Email>.Some || phoneNo is Maybe<PhoneNo>.Some;
    

}