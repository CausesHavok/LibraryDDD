using LibraryDDD.Shared.Common;
using Xunit.Sdk;

namespace LibraryDDD.Shared.Tests;

public class ResultExtensionsTests
{
    [Fact]
    public void MapError_WhenResultIsSuccessful_PreservesValueAndDoesNotMapError()
    {
        var mapWasCalled = false;
        var result = Result<int, string>.Ok(42);

        var mapped = result.MapError(error =>
        {
            mapWasCalled = true;
            return error.Length;
        });

        Assert.True(mapped.IsSuccess);
        Assert.Equal(42, mapped.Value);
        Assert.False(mapWasCalled);
    }

    [Fact]
    public void MapError_WhenResultIsFailure_MapsError()
    {
        var result = Result<int, string>.Fail("invalid");

        var mapped = result.MapError(error => error.Length);

        Assert.False(mapped.IsSuccess);
        Assert.Equal("invalid".Length, mapped.Error);
    }

    [Fact]
    public void MapError_WhenResultIsNull_ThrowsError()
    {
        Result<int, string>? result = null;

        Assert.Throws<ArgumentNullException>(() => _ = result!.MapError(error => error.Length));
    }

    [Fact]
    public void MapError_WhenMapIsNull_ThrowsError()
    {
        var result = Result<int, string>.Fail("invalid");
        Func<string, int>? map = null;

        Assert.Throws<ArgumentNullException>(() => result.MapError(map!));
    }

    [Fact]
    public void ResultOfValue_WhenValueAndErrorTypesMatch_ExposesCorrectPayload()
    {
        var success = Result<string, string>.Ok("value");
        var failure = Result<string, string>.Fail("error");

        Assert.Equal("value", success.Value);
        Assert.Equal("error", failure.Error);
    }
}