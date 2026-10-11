using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Domain.Validation;

namespace LibraryDDD.Domain.Tests;

public class ContactInformationTests
{
    [Fact]
    public void TryCreate_NoEmailOrPhone_ReturnsFailure()
    {
        var result = ContactInformation.TryCreate(
            new Maybe<Email>.None(),
            new Maybe<PhoneNo>.None());

        Assert.False(result.IsSuccess);
        Assert.Equal(ContactInformationError.MissingEmailAndPhoneNo, result.Error);
    }

    [Fact]
    public void TryCreate_EmailOnly_ReturnsSuccess()
    {
        var email = Email.TryCreate("person@example.com").Value;

        var result = ContactInformation.TryCreate(
            new Maybe<Email>.Some(email),
            new Maybe<PhoneNo>.None());

        Assert.True(result.IsSuccess);
        Assert.Equal(email, Assert.IsType<Maybe<Email>.Some>(result.Value.Email).Value);
        Assert.IsType<Maybe<PhoneNo>.None>(result.Value.PhoneNo);
    }

    [Fact]
    public void TryCreate_PhoneOnly_ReturnsSuccess()
    {
        var phone = PhoneNo.TryCreate("+4512345678").Value;

        var result = ContactInformation.TryCreate(
            new Maybe<Email>.None(),
            new Maybe<PhoneNo>.Some(phone));

        Assert.True(result.IsSuccess);
        Assert.IsType<Maybe<Email>.None>(result.Value.Email);
        Assert.Equal(phone, Assert.IsType<Maybe<PhoneNo>.Some>(result.Value.PhoneNo).Value);
    }

    [Fact]
    public void TryCreate_EmailAndPhone_ReturnsSuccess()
    {
        var email = Email.TryCreate("person@example.com").Value;
        var phone = PhoneNo.TryCreate("+4512345678").Value;

        var result = ContactInformation.TryCreate(
            new Maybe<Email>.Some(email),
            new Maybe<PhoneNo>.Some(phone));

        Assert.True(result.IsSuccess);
        Assert.Equal(email, Assert.IsType<Maybe<Email>.Some>(result.Value.Email).Value);
        Assert.Equal(phone, Assert.IsType<Maybe<PhoneNo>.Some>(result.Value.PhoneNo).Value);
    }
}
