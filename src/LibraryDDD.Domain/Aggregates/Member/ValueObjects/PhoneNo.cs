namespace LibraryDDD.Domain.Aggregates.Member;

internal sealed record PhoneNo
{
    public string Value { get; }

    public PhoneNo(string value)
    {
        if (!IsValidPhoneNo(value))
            throw new ArgumentException("Invalid phone number.", nameof(value));
        
        Value = value;
    }

    private static bool IsValidPhoneNo(string phoneNo)
    {
        // Simple validation: checks if the phone number contains only digits and optional '+' at the start
        if (string.IsNullOrWhiteSpace(phoneNo))
            return false;

        if (phoneNo[0] == '+')
            phoneNo = phoneNo.Substring(1);

        return phoneNo.All(char.IsDigit);
    }
}