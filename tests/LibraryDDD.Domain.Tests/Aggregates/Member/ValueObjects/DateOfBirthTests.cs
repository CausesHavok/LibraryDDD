using LibraryDDD.Domain.Aggregates.Member;
using LibraryDDD.Domain.Validation;

namespace LibraryDDD.Domain.Tests;

public class DateOfBirthTests
{
    [Theory]
    [InlineData(1900, 1, 1)]
    [InlineData(2000, 2, 29)]
    public void TryCreate_ValidDate_ReturnsDateOfBirth(int year, int month, int day)
    {
        var date = new DateOnly(year, month, day);

        var result = DateOfBirth.TryCreate(date);

        Assert.True(result.IsSuccess);
        Assert.Equal(date, result.Value.Value);
    }

    [Fact]
    public void TryCreate_Today_ReturnsSuccess()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        var result = DateOfBirth.TryCreate(today);

        Assert.True(result.IsSuccess);
        Assert.Equal(today, result.Value.Value);
    }

    [Fact]
    public void TryCreate_FutureDate_ReturnsFutureDateError()
    {
        var tomorrow = DateOnly.FromDateTime(DateTime.Now).AddDays(1);

        var result = DateOfBirth.TryCreate(tomorrow);

        Assert.False(result.IsSuccess);
        Assert.Equal(DateOfBirthError.FutureDate, result.Error);
    }

    [Fact]
    public void TryCreate_DateBefore1900_ReturnsInvalidDateError()
    {
        var result = DateOfBirth.TryCreate(new DateOnly(1899, 12, 31));

        Assert.False(result.IsSuccess);
        Assert.Equal(DateOfBirthError.InvalidDate, result.Error);
    }
}
