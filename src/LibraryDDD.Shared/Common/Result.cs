namespace LibraryDDD.Shared.Common;

public sealed class Result<E>
{
    public bool IsSuccess { get; }

    private readonly E? _error;
    public E Error => !IsSuccess
        ? _error!
        : throw new InvalidOperationException("Cannot read Value of a failed result.");

    private Result(bool isSuccess, E error)
    {
        IsSuccess = isSuccess;
        _error = error;
    }

    public static Result<E> Fail(E error) => new(false, error);
    public static Result<E> Ok() => new(true, default!);
}

public sealed class Result<T, E>
{
    public bool IsSuccess { get; }

    private readonly T? _value;
    private readonly E? _error;

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot read Value of a failed result.");

    public E Error => !IsSuccess
        ? _error!
        : throw new InvalidOperationException("Cannot read Error of a successful result.");

    private Result(T value)
    {
        IsSuccess = true;
        _value = value;
        _error = default!;
    }

    private Result(E error)
    {
        IsSuccess = false;
        _error = error;
        _value = default!;
    }

    public static Result<T, E> Ok(T value) => new(value);
    public static Result<T, E> Fail(E error) => new(error);
}