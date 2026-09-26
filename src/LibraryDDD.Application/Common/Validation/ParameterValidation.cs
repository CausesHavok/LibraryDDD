namespace LibraryDDD.Application.Common.Validation;

public static class ParameterValidation
{
    public static bool AreNoneSet(params object?[] parameters)
        => parameters.All(p => !IsSet(p));

    public static bool AreAllSet(params object?[] parameters)
        => parameters.All(IsSet);
    
    public static bool IsOnlyOneSet(params object?[] parameters)
        => parameters.Count(IsSet) == 1;
    
    public static bool AreAtLeastOneSet(params object?[] parameters)
        => parameters.Any(IsSet);

    private static bool IsSet(object? parameter)
        => parameter switch
        {
            null => false,
            string str when string.IsNullOrWhiteSpace(str) => false,
            _ => true
        };
}