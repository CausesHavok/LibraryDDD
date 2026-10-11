namespace LibraryDDD.Api.Parsing.Tests;

public class ApiDateOnlyParserTests
{
    [Theory]
    [InlineData("2023-02-30")] // Invalid date (February 30th)
    [InlineData("2023-13-01")] // Invalid month (13)
    [InlineData("2023-00-10")] // Invalid month (0)
    [InlineData("2023-01-00")] // Invalid day (0)
    [InlineData("2023-01-32")] // Invalid day (32)
    [InlineData("2023-1-1")]   // Invalid format (single digit month and day)
    [InlineData("2023/01/01")] // Invalid format (slashes instead of dashes)
    [InlineData("01-01-2023")] // Invalid format (day-month-year)
    [InlineData("2023-01-01T00:00:00")] // Invalid format (includes time)
    public void TryParse_InvalidDate_ReturnsFalse(string inputDate)
    {
        // Act
        var result = ApiDateOnlyParser.TryParse(inputDate, out var parsedDate);

        // Assert
        Assert.False(result);
        Assert.Equal(default(DateOnly), parsedDate);
    }

    [Theory]
    [InlineData("1500-04-17", 1500, 4, 17)]     // Valid date in the past
    [InlineData("2023-10-01", 2023, 10, 1)]     // Valid date in the present
    [InlineData("2023-12-31", 2023, 12, 31)]    // Valid date in the present
    [InlineData("2024-02-29", 2024, 2, 29)]     // Leap year
    [InlineData("2030-12-31", 2030, 12, 31)]    // Valid date in the future
    public void TryParse_ValidDate_ReturnsTrueAndCorrectDateOnly(string inputDate, int year, int month, int day)
    {
        // Arrange
        var expectedDate = new DateOnly(year, month, day);

        // Act
        var result = ApiDateOnlyParser.TryParse(inputDate, out var parsedDate);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedDate, parsedDate);
    }

    [Theory]
    [InlineData("1500-04-17", 1500, 4, 17)]     // Valid date in the past
    [InlineData("2023-10-01", 2023, 10, 1)]      // Valid date in the present
    [InlineData("2023-12-31", 2023, 12, 31)]    // Valid date in the present
    [InlineData("2024-02-29", 2024, 2, 29)]     // Leap year 
    [InlineData("2030-12-31", 2030, 12, 31)]    // Valid date in the future
    public void Parse_ValidDate_ReturnsCorrectDateOnly(string inputDate, int year, int month, int day)
    {
        // Arrange
        var expectedDate = new DateOnly(year, month, day);

        // Act
        var result = ApiDateOnlyParser.Parse(inputDate);

        // Assert
        Assert.Equal(expectedDate, result);
    }

    [Theory]
    [InlineData("2023-02-30")] // Invalid date (February 30th)
    [InlineData("2023-13-01")] // Invalid month (13)
    [InlineData("2023-00-10")] // Invalid month (0)
    [InlineData("2023-01-00")] // Invalid day (0)
    [InlineData("2023-01-32")] // Invalid day (32)
    [InlineData("2023-1-1")]   // Invalid format (single digit month and day)
    [InlineData("2023/01/01")] // Invalid format (slashes instead of dashes)
    [InlineData("01-01-2023")] // Invalid format (day-month-year)
    [InlineData("2023-01-01T00:00:00")] // Invalid format (includes time)
    public void Parse_InvalidDate_ThrowsFormatException(string inputDate)
    {
        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => ApiDateOnlyParser.Parse(inputDate));
        Assert.Equal($"Invalid date format. Expected yyyy-MM-dd.", exception.Message);
    }
}