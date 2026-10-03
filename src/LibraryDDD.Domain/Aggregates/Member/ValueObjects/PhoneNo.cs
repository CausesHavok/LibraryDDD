using LibraryDDD.Domain.Validation;
using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Shared.Common;

namespace LibraryDDD.Domain.Aggregates.Member;

public sealed record PhoneNo
{
    public NonEmptyString Value { get; }

    private PhoneNo(NonEmptyString value)
    {   
        Value = value;
    }

    public static Result<PhoneNo, PhoneNoError> TryCreate(NonEmptyString value)
    {
        if (!IsValidPhoneNo(value))
            return Result<PhoneNo, PhoneNoError>.Fail(PhoneNoError.InvalidFormat);

        return Result<PhoneNo, PhoneNoError>.Ok(new PhoneNo(value));
    }

    private static bool IsValidPhoneNo(NonEmptyString phoneNo)
    {
        // Simple validation: checks if the phone number contains only digits and optional '+' at the start
        string phoneNoValue = phoneNo.Value;
        if (phoneNoValue[0] == '+')
            phoneNoValue = phoneNoValue[1..];

        return phoneNoValue.All(char.IsDigit);
    }
}