namespace CoreMachine.Result;

public static class ResultExtensions
{
	public static Result<T, TE> ValueOr<T, TE>(this T? value, TE error) => value is not null ? value : error;

	public static Task<Result<T, TE>> TaskToValue<T, TE>(this Task<T> task)
		=> task.ContinueWith(t => Result.Ok<T, TE>(t.Result));

	public static Task<Result<T, TE>> TaskToError<T, TE>(this Task<TE> task)
		=> task.ContinueWith(t => Result.Err<T, TE>(t.Result));

	public static Task<Result<TNew, TError>> MapAsync<T, TNew, TError>(
		this Task<Result<T, TError>> task,
		Func<T, Task<TNew>> mapper)
		=> task
			.ContinueWith(t => t.Result.MapAsync(mapper),
				TaskContinuationOptions.OnlyOnRanToCompletion & TaskContinuationOptions.NotOnCanceled)
			.Unwrap();
}