namespace LibraryDDD.Shared.Common;

public static class NullChecks
{
    
    public static bool AllNull(params object?[] values)
        => values.All(v => v is null);

    public static bool NoneNull(params object?[] values)
        => values.All(v => v is not null);

    public static bool AnyNull(params object?[] values)
        => values.Any(v => v is null);
}