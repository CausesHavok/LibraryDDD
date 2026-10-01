namespace LibraryDDD.Domain.Aggregates.Member;

internal sealed record Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email.", nameof(value));
        
        Value = value;
    }

    private static bool IsValidEmail(string email) 
        => System.Net.Mail.MailAddress.TryCreate(email, out _);
}