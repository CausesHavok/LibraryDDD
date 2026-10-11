using LibraryDDD.Shared.Common;

namespace LibraryDDD.Shared.Tests;

public class ResultTests
{
    [Fact]
    public void ResultOfError_Ok_SetsSuccessState()
    {
        var result = Result<string>.Ok();

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ResultOfError_Fail_SetsFailureState()
    {
        var result = Result<string>.Fail("bad");

        Assert.False(result.IsSuccess);
        Assert.Equal("bad", result.Error);
    }

    [Fact]
    public void ResultOfError_WhenSuccess_ReadingErrorThrows()
    {
        var result = Result<string>.Ok();
        Assert.Throws<InvalidOperationException>(() => _ = result.Error);
    }

    [Fact]
    public void ResultOfValue_Ok_ExposesValue()
    {
        var result = Result<int, string>.Ok(42);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void ResultOfValue_Fail_ExposesError()
    {
        var result = Result<int, string>.Fail("invalid");

        Assert.False(result.IsSuccess);
        Assert.Equal("invalid", result.Error);
    }

    [Fact]
    public void ResultOfValue_WhenSuccess_ReadingErrorThrows()
    {
        var result = Result<int, string>.Ok(42);

        Assert.Throws<InvalidOperationException>(() => _ = result.Error);
    }

    [Fact]
    public void ResultOfValue_WhenFailure_ReadingValueThrows()
    {
        var result = Result<int, string>.Fail("invalid");

        Assert.Throws<InvalidOperationException>(() => _ = result.Value);
    }
}