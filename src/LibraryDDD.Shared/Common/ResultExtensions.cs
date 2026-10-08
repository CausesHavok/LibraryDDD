namespace LibraryDDD.Shared.Common;

public static class ResultExtensions
{
    public static Result<T, E2> MapError<T, E, E2>(this Result<T, E> result, Func<E, E2> map) =>
        result.IsSuccess
            ? Result<T, E2>.Ok(result.Value)
            : Result<T, E2>.Fail(map(result.Error));

}