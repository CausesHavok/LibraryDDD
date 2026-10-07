using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Domain.Validation;
using LibraryDDD.Shared.Common;

namespace LibraryDDD.Domain.Aggregates.Member;

public sealed record Email
{
    public NonEmptyString Value { get; }

    private Email(NonEmptyString value)
    {
        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email.", nameof(value));
        
        Value = value;
    }

    public static Result<Email, EmailError> TryCreate(NonEmptyString value)
    {
        if (!IsValidEmail(value))
            return Result<Email, EmailError>.Fail(EmailError.InvalidFormat);

        return Result<Email, EmailError>.Ok(new Email(value));
    }

    public static Result<Email, EmailError> TryCreate(string value)
    {
        var nonEmptyEmail = NonEmptyString.TryCreate(value);
        if (!nonEmptyEmail.IsSuccess)
            return Result<Email, EmailError>.Fail(MapError(nonEmptyEmail.Error));

        return TryCreate(nonEmptyEmail.Value);
    }

    private static bool IsValidEmail(NonEmptyString email) 
        => System.Net.Mail.MailAddress.TryCreate(email.ToString(), out _);

    private static EmailError MapError(NonEmptyStringError error) =>
        #pragma warning disable CS8524
        error switch
        {
            NonEmptyStringError.Empty => EmailError.Empty,
        };
        #pragma warning restore CS8524
}