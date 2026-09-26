namespace LibraryDDD.Domain.Aggregates.Member;

internal sealed record StaffId
{
    public string Value { get; }

    public StaffId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Empty staff ID.", nameof(value));

        Value = value;
    }
}