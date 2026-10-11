using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Domain.Validation;

namespace LibraryDDD.Domain.Tests;

public class PhoneNoTests
{
    [Theory]
    [InlineData("1234567890")]
    [InlineData("+4512345678")]
    public void TryCreate_ValidPhoneNumber_ReturnsPhoneNumber(string value)
    {
        var result = PhoneNo.TryCreate(value);

        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value.Value);
    }

    [Theory]
    [InlineData("123-456")]
    [InlineData("12a34")]
    [InlineData("++123")]
    [InlineData("12+3")]
    public void TryCreate_InvalidPhoneNumber_ReturnsInvalidFormatError(string value)
    {
        var result = PhoneNo.TryCreate(value);

        Assert.False(result.IsSuccess);
        Assert.Equal(PhoneNoError.InvalidFormat, result.Error);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void TryCreate_EmptyPhoneNumber_ReturnsEmptyError(string value)
    {
        var result = PhoneNo.TryCreate(value);

        Assert.False(result.IsSuccess);
        Assert.Equal(PhoneNoError.Empty, result.Error);
    }
}
