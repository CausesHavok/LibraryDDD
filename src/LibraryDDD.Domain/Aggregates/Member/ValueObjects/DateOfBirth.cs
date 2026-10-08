using System.Reflection.Metadata.Ecma335;
using LibraryDDD.Domain.Validation;
using LibraryDDD.Shared.Common;

namespace LibraryDDD.Domain.Aggregates.Member;

public record DateOfBirth
{
    public DateOnly Value {get;}

    private DateOfBirth(DateOnly value)
    {
        Value = value;
    }


    public static Result<DateOfBirth, DateOfBirthError> TryCreate(DateOnly value)
    {
        if (value > DateOnly.FromDateTime(DateTime.Now))
            return Result<DateOfBirth, DateOfBirthError>.Fail(DateOfBirthError.FutureDate);
        if (value < new DateOnly(1900,1,1))
            return Result<DateOfBirth, DateOfBirthError>.Fail(DateOfBirthError.InvalidDate);
        return Result<DateOfBirth, DateOfBirthError>.Ok(new DateOfBirth(value));
    }

}