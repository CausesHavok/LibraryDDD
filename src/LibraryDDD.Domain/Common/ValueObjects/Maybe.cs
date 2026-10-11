namespace LibraryDDD.Domain.Common.ValueObjects;

public abstract record Maybe<T>
{
    private Maybe() { }

    public sealed record Some(T Value) : Maybe<T>;

    public sealed record None : Maybe<T>;

    public static Maybe<T> From(T? value)
        => value is null ? new None() : new Some(value);
}
