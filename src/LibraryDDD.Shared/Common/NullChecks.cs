using System.Windows.Markup;

namespace LibraryDDD.Shared.Common;

public static class NullChecks
{
    
    public static bool AllNull(params object?[] values)
    {
        ValidateInput(values);        
        return values.All(v => v is null);
    }

    public static bool NoneNull(params object?[] values)
    {
        ValidateInput(values); 
        return values.All(v => v is not null);
    }

    public static bool AnyNull(params object?[] values)
    {
        ValidateInput(values); 
        return values.Any(v => v is null);
    }

    private static void ValidateInput(params object?[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentOutOfRangeException.ThrowIfEqual(0, values.Length);
    }
}