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
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}