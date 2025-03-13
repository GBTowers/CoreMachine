namespace CoreMachine.Result;

public static class ResultExtensions
{
    public static Result<T, TE> ValueOr<T, TE>(this T? value, TE error) => value is not null ? value : error;
}