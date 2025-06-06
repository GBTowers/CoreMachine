// ReSharper disable UnusedMember.Global

namespace CoreMachine.Result;

public static class ResultExtensions
{
	public static Result<T, TE> ValueOr<T, TE>(this T? value, TE error) => value is not null ? value : error;

	public static async Task<TOut> Match<T, TE, TOut>(
		this Task<Result<T, TE>> task,
		Func<T, TOut> ok,
		Func<TE, TOut> err
	)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		return result.Match(ok: ok, err: err);
	}

	public static async Task<TOut> MatchAsync<T, TE, TOut>(
		this Task<Result<T, TE>> task,
		Func<T, Task<TOut>> ok,
		Func<TE, Task<TOut>> err
	)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		return await result.MatchAsync(ok: ok, err: err);
	}

	public static async Task Switch<T, TE>(
		this Task<Result<T, TE>> task,
		Action<T> ok,
		Action<TE> err
	)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		result.Switch(ok: ok, err: err);
	}

	public static async Task SwitchAsync<T, TE>(
		this Task<Result<T, TE>> task,
		Func<T, Task> ok,
		Func<TE, Task> err
	)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		await result.SwitchAsync(ok: ok, err: err);
	}

	public static async Task<Result<TNew, TE>> Map<T, TE, TNew>(this Task<Result<T, TE>> task, Func<T, TNew> next)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		return result.Map(next);
	}

	public static async Task<Result<TNew, TE>> MapAsync<T, TE, TNew>(
		this Task<Result<T, TE>> task,
		Func<T, Task<TNew>> next
	)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		return await result.MapAsync(next);
	}

	public static async Task<Result<T, TNew>> MapError<T, TE, TNew>(this Task<Result<T, TE>> task, Func<TE, TNew> next)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		return result.MapError(next);
	}

	public static async Task<Result<T, TNew>> MapErrorAsync<T, TE, TNew>(
		this Task<Result<T, TE>> task,
		Func<TE, Task<TNew>> next
	)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		return await result.MapErrorAsync(next);
	}

	public static async Task<Result<TNew, TE>> Bind<T, TE, TNew>(
		this Task<Result<T, TE>> task,
		Func<T, Result<TNew, TE>> next
	)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		return result.Bind(next);
	}

	public static async Task<Result<TNew, TE>> BindAsync<T, TE, TNew>(
		this Task<Result<T, TE>> task,
		Func<T, Task<Result<TNew, TE>>> next
	)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		return await result.BindAsync(next);
	}

	public static async Task<Result<T, TNew>> BindError<T, TE, TNew>(
		this Task<Result<T, TE>> task,
		Func<TE, Result<T, TNew>> next
	)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		return result.BindError(next);
	}

	public static async Task<Result<T, TNew>> BindErrorAsync<T, TE, TNew>(
		this Task<Result<T, TE>> task,
		Func<TE, Task<Result<T, TNew>>> next
	)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		return await result.BindErrorAsync(next);
	}

	public static async Task<Result<T, TE>> Assert<T, TE>(
		this Task<Result<T, TE>> task,
		Func<T, bool> assertion,
		TE error
	)
	{
		Result<T, TE> result = await task.ConfigureAwait(false);
		return result.Assert(assert: assertion, error: error);
	}
}
