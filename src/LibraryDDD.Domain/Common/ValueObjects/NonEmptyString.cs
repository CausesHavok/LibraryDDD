using LibraryDDD.Shared.Common;
using LibraryDDD.Domain.Validation;
namespace LibraryDDD.Domain.Common.ValueObjects;

public sealed record NonEmptyString
{
    public string Value{ get; }
    
    private NonEmptyString(string value)
    {
        Value = value;
    }

    public static Result<NonEmptyString, NonEmptyStringError> TryCreate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<NonEmptyString, NonEmptyStringError>.Fail(NonEmptyStringError.Empty);

        return Result<NonEmptyString, NonEmptyStringError>.Ok(new NonEmptyString(value));
    }
}