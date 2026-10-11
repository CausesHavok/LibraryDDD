using LibraryDDD.Domain.Common.ValueObjects;

namespace LibraryDDD.Domain.Tests;

public class MaybeTests
{
    [Fact]
    public void From_ValidValue_ReturnsSomeWithValue()
    {
        var result = Maybe<int>.From(42);

        var some = Assert.IsType<Maybe<int>.Some>(result);
        Assert.Equal(42, some.Value);
    }

    [Fact]
    public void From_NullReferenceValue_ReturnsNone()
    {
        var result = Maybe<string>.From((string?)null);

        Assert.IsType<Maybe<string>.None>(result);
    }

    [Fact]
    public void From_EmptyStringValue_ReturnsSomeWithValue()
    {
        var result = Maybe<string>.From(string.Empty);

        var some = Assert.IsType<Maybe<string>.Some>(result);
        Assert.Equal(string.Empty, some.Value);
    }
}
