using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Domain.Validation;

namespace LibraryDDD.Domain.Tests;

public class StaffIdTests
{
    [Fact]
    public void TryCreate_ValidValue_ReturnsStaffId()
    {
        var result = StaffId.TryCreate("STAFF-123");

        Assert.True(result.IsSuccess);
        Assert.Equal("STAFF-123", result.Value.Value.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void TryCreate_EmptyValue_ReturnsEmptyError(string value)
    {
        var result = StaffId.TryCreate(value);

        Assert.False(result.IsSuccess);
        Assert.Equal(StaffIdError.Empty, result.Error);
    }
}
