using LibraryDDD.Domain.Validation;
using LibraryDDD.Shared.Common;

namespace LibraryDDD.Domain.Aggregates.Member;

internal sealed record Email
{
    public string Value { get; }

    private Email(string value)
    {
        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email.", nameof(value));
        
        Value = value;
    }

    public static Result<Email, EmailError> TryCreate(string value)
    {
        if (!IsValidEmail(value))
            return Result<Email, EmailError>.Fail(EmailError.InvalidFormat);

        return Result<Email, EmailError>.Ok(new Email(value));
    }

    private static bool IsValidEmail(string email) 
        => System.Net.Mail.MailAddress.TryCreate(email, out _);
}