using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Shared.Common;
using LibraryDDD.Domain.Validation;

namespace LibraryDDD.Domain.Aggregates.Member;

public sealed record StaffId
{
    
    public NonEmptyString Value;

    private StaffId(NonEmptyString value)
    {
        Value = value;
    }

    public static Result<StaffId, StaffIdError> TryCreate(NonEmptyString value)
    {
        return Result<StaffId, StaffIdError>.Ok(new StaffId(value));
    }


    public static Result<StaffId, StaffIdError> TryCreate(string value)
    {
        var nonEmptyString = NonEmptyString.TryCreate(value);
        if (!nonEmptyString.IsSuccess)
            return Result<StaffId, StaffIdError>.Fail(MapError(nonEmptyString.Error));

        return TryCreate(nonEmptyString.Value);
    }

    private static StaffIdError MapError(NonEmptyStringError error) =>
        #pragma warning disable CS8524
        error switch
        {
            NonEmptyStringError.Empty => StaffIdError.Empty
        };
        #pragma warning restore CS8524

}