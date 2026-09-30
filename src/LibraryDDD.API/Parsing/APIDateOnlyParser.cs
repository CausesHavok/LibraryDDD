using System.Globalization;

namespace LibraryDDD.Api.Parsing;
internal static class ApiDateOnlyParser
{
    private const string Format = "yyyy-MM-dd";

    public static DateOnly Parse(string date)
    {
        if (TryParse(date, out var result))
            return result;

        throw new FormatException($"Invalid date format. Expected {Format}.");
    }

    public static bool TryParse(string? date, out DateOnly result) =>
        DateOnly.TryParseExact(
            date,
            Format,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out result);
}