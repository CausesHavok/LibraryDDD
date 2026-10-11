using LibraryDDD.Domain.Common.ValueObjects;
using LibraryDDD.Domain.Validation;

namespace LibraryDDD.Domain.Tests;

public class NonEmptyStringTests
{
    [Theory]
    [InlineData("abc")]
    [InlineData(" hello ")]
    [InlineData("123")]
    public void TryCreate_ValidValue_ReturnsSuccessAndValue(string value)
    {
        var result = NonEmptyString.TryCreate(value);

        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\r\n")]
    public void TryCreate_WhitespaceValue_ReturnsFailure(string value)
    {
        var result = NonEmptyString.TryCreate(value);

        Assert.False(result.IsSuccess);
        Assert.Equal(NonEmptyStringError.Empty, result.Error);
    }

    [Fact]
    public void TryCreate_NullValue_ReturnsFailure()
    {
        var result = NonEmptyString.TryCreate(null!);

        Assert.False(result.IsSuccess);
        Assert.Equal(NonEmptyStringError.Empty, result.Error);
    }

    [Fact]
    public void TryCreate_SameValue_ReturnsEqualInstances()
    {
        var first = NonEmptyString.TryCreate("LibraryDDD");
        var second = NonEmptyString.TryCreate("LibraryDDD");

        Assert.True(first.IsSuccess);
        Assert.True(second.IsSuccess);
        Assert.Equal(first.Value, second.Value);
    }
}
