using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Domain.Validation;

namespace LibraryDDD.Domain.Tests;

public class EmailTests
{
    [Theory]
    [InlineData("person@example.com")]
    [InlineData("person+tag@example.co.uk")]
    public void TryCreate_ValidEmail_ReturnsEmail(string value)
    {
        var result = Email.TryCreate(value);

        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value.Value);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("person@")]
    public void TryCreate_InvalidEmail_ReturnsInvalidFormatError(string value)
    {
        var result = Email.TryCreate(value);

        Assert.False(result.IsSuccess);
        Assert.Equal(EmailError.InvalidFormat, result.Error);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void TryCreate_EmptyEmail_ReturnsEmptyError(string value)
    {
        var result = Email.TryCreate(value);

        Assert.False(result.IsSuccess);
        Assert.Equal(EmailError.Empty, result.Error);
    }
}
