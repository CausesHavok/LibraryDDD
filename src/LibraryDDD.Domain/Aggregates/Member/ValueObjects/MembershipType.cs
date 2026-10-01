using LibraryDDD.Domain.Common;

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

    public static MembershipType FromString(string value)
        => value switch
        {
            "Adult" => Adult,
            "Child" => Child,
            "Staff" => Staff,
            _ => throw new ArgumentException("Invalid membership type.", nameof(value)),
        };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
