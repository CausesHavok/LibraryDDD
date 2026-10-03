using LibraryDDD.Domain.Common;
using LibraryDDD.Domain.Validation;
using LibraryDDD.Shared.Common;

namespace LibraryDDD.Domain.Aggregates.Member;

internal sealed class MembershipType : ValueObject
{
    public string Value { get; }

    private MembershipType(string value)
        => Value = value;

    // Singletons — one instance per membership type
    public static readonly MembershipType Adult = new("Adult");
    public static readonly MembershipType Child = new("Child");
    public static readonly MembershipType Staff = new("Staff");

    public static Result<MembershipType, MembershipTypeError> TryCreate(string value)
        => value switch
        {
            "Adult" => Result<MembershipType, MembershipTypeError>.Ok(Adult),
            "Child" => Result<MembershipType, MembershipTypeError>.Ok(Child),
            "Staff" => Result<MembershipType, MembershipTypeError>.Ok(Staff),
            _ => Result<MembershipType, MembershipTypeError>.Fail(MembershipTypeError.InvalidType),
        };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
