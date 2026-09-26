namespace LibraryDDD.Domain.Common.ValueObjects;

internal sealed record NonEmptyString
{
    public string Value{ get; }
    public NonEmptyString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value must not be empty.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}