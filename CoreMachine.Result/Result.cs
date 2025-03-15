// ReSharper disable UnusedMember.Global
namespace CoreMachine.Result;

/// <summary>
/// Result type that represents the union between <see cref="T"/>
/// and <see cref="TError"/> with a bunch of methods for error handling
/// </summary>
/// <typeparam name="T">The type of the success path</typeparam>
/// <typeparam name="TError">The type of the error path</typeparam>
public abstract record Result<T, TError>
{
	private protected abstract T InternalValue { get; }
	private protected abstract TError InternalError { get; }

	private protected abstract bool IsError { get; }

	/// <summary>
	/// Matches the <see cref="Result{T,TError}"/> state and executes the corresponding delegate,
	/// mapping both possibilities into a single type
	/// </summary>
	/// <param name="ok">The function for the success path</param>
	/// <param name="err">The function for the failure path</param>
	/// <typeparam name="TOut">The mapped result type</typeparam>
	/// <returns>The result of either delegate</returns>
	public TOut Match<TOut>(Func<T, TOut> ok, Func<TError, TOut> err)
		=> IsError ? err(InternalError) : ok(InternalValue);

	/// <summary>
	/// Matches the <see cref="Result{T,TError}"/> state and executes
	/// the corresponding asynchronous delegate,
	/// mapping both possibilities into a single type
	/// </summary>
	/// <param name="ok">The asynchronous function for the success path</param>
	/// <param name="err">The asynchronous function for the failure path</param>
	/// <typeparam name="TOut">The mapped result type</typeparam>
	/// <returns>the <see cref="Task{TResult}"/> containing the result of either delegate</returns>
	public Task<TOut> MatchAsync<TOut>(Func<T, Task<TOut>> ok, Func<TError, Task<TOut>> err)
		=> IsError ? err(InternalError) : ok(InternalValue);

	/// <summary>
	/// Matches the <see cref="Result{T,TError}"/> state and executes
	/// the corresponding action
	/// </summary>
	/// <param name="ok">The action for the success path</param>
	/// <param name="err">The action for the failure path</param>
	public void Switch(Action<T> ok, Action<TError> err)
	{
		if (IsError)
		{
			err(InternalError);
			return;
		}

		ok(InternalValue);
	}

	/// <summary>
	/// Matches the <see cref="Result{T,TError}"/> state and executes
	/// the corresponding asynchronous action
	/// </summary>
	/// <param name="ok">The asynchronous action for the success path</param>
	/// <param name="err">The asynchronous action for the failure path</param>
	public async Task SwitchAsync(Func<T, Task> ok, Func<TError, Task> err)
	{
		if (IsError)
		{
			await err(InternalError);
			return;
		}

		await ok(InternalValue);
	}

	/// <summary>
	/// Maps the <see cref="TError"/> into a new type,
	/// leaving the <see cref="T"/> type intact
	/// </summary>
	/// <param name="next">The error type mapper function</param>
	/// <typeparam name="TNew">The new error type</typeparam>
	/// <returns>A new <see cref="Result{T,TNew}"/> with <see cref="TNew"/> as the new error type</returns>
	public Result<T, TNew> MapError<TNew>(Func<TError, TNew> next)
		=> IsError ? next(InternalError) : InternalValue;

	/// <summary>
	/// Maps the <see cref="TError"/> into a new type asynchronously,
	/// leaving the <see cref="T"/> type intact
	/// </summary>
	/// <param name="next">The asynchronous error type mapper function</param>
	/// <typeparam name="TNew">The new error type</typeparam>
	/// <returns>
	/// A <see cref="Task{T}"/> containing a new <see cref="Result{T,TNew}"/> with <see cref="TNew"/>
	/// as the new error type
	/// </returns>
	public Task<Result<T, TNew>> MapErrorAsync<TNew>(Func<TError, Task<TNew>> next)
		=> IsError ? next(InternalError).TaskToError<T, TNew>() : Task.FromResult<Result<T, TNew>>(InternalValue);

	/// <summary>
	/// Maps the <see cref="T"/> into a new type,
	/// leaving the <see cref="TError"/> type intact
	/// </summary>
	/// <param name="next">The type mapper function</param>
	/// <typeparam name="TNew">The new value type</typeparam>
	/// <returns>A new <see cref="Result{T,TNew}"/> with <see cref="TNew"/> as the new value type</returns>
	public Result<TNew, TError> Map<TNew>(Func<T, TNew> next)
		=> !IsError ? next(InternalValue) : InternalError;

	/// <summary>
	/// Maps the <see cref="T"/> into a new type asynchronously,
	/// leaving the <see cref="TError"/> type intact
	/// </summary>
	/// <param name="next">The asynchronous type mapper function</param>
	/// <typeparam name="TNew">The new value type</typeparam>
	/// <returns>
	/// A <see cref="Task{T}"/> containing a new <see cref="Result{T,TNew}"/> with <see cref="TNew"/>
	/// as the new value type
	/// </returns>
	public Task<Result<TNew, TError>> MapAsync<TNew>(Func<T, Task<TNew>> next)
		=> !IsError
			? next(InternalValue).TaskToValue<TNew, TError>()
			: Task.FromResult(Result.Err<TNew, TError>(InternalError));


	/// <summary>
	/// Binds another function returning <see cref="Result{T,TError}"/>
	/// with <see cref="TNew"/> as the new value type.
	/// The binded function only executes if the <see cref="Result{T,TError}"/>
	/// state is success, otherwise it returns the current error
	/// </summary>
	/// <param name="next">Binded function</param>
	/// <typeparam name="TNew">The new value type for the <see cref="Result{T,TError}"/></typeparam>
	/// <returns>A new <see cref="Result{T,TError}"/> with the new value type</returns>
	public Result<TNew, TError> Bind<TNew>(Func<T, Result<TNew, TError>> next)
		=> !IsError ? next(InternalValue) : InternalError;

	/// <summary>
	/// Binds another asynchronous function returning <see cref="Result{T,TError}"/>
	/// with <see cref="TNew"/> as the new value type.
	/// The binded asynchronous function only executes if the <see cref="Result{T,TError}"/>
	/// state is success, otherwise it returns the current error
	/// </summary>
	/// <param name="next">Binded asynchronous function</param>
	/// <typeparam name="TNew">The new value type for the <see cref="Result{T,TError}"/></typeparam>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a new <see cref="Result{T,TError}"/> with the new value type
	/// </returns>
	public Task<Result<TNew, TError>> BindAsync<TNew>(Func<T, Task<Result<TNew, TError>>> next)
		=> !IsError ? next(InternalValue) : Task.FromResult(Result.Err<TNew, TError>(InternalError));

	/// <summary>
	/// Binds another function returning <see cref="Result{T,TError}"/>
	/// with <see cref="TNewError"/> as the new error type.
	/// The binded function only executes if the <see cref="Result{T,TError}"/>
	/// state is success, otherwise it returns the current error
	/// </summary>
	/// <param name="next">Binded function</param>
	/// <typeparam name="TNewError">The new error type for the <see cref="Result{T,TError}"/></typeparam>
	/// <returns>A new <see cref="Result{T,TError}"/> with the new error type</returns>
	public Result<T, TNewError> BindError<TNewError>(Func<TError, Result<T, TNewError>> next)
		=> IsError ? next(InternalError) : InternalValue;

	/// <summary>
	/// Binds another asynchronous function returning <see cref="Result{T,TError}"/>
	/// with <see cref="TNewError"/> as the new error type.
	/// The binded asynchronous function only executes if the <see cref="Result{T,TError}"/>
	/// state is success, otherwise it returns the current error
	/// </summary>
	/// <param name="next">Binded asynchronous function</param>
	/// <typeparam name="TNewError">The new error type for the <see cref="Result{T,TError}"/></typeparam>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a new <see cref="Result{T,TError}"/> with the new error type
	/// </returns>
	public async Task<Result<T, TNewError>> BindErrorAsync<TNewError>(
		Func<TError, Task<Result<T, TNewError>>> next)
		=> IsError ? await next(InternalError).ConfigureAwait(false) : InternalValue;

	/// <summary>
	/// Executes a given delegate-like function if the <see cref="Result{T,TError}"/> state is success,
	/// if the result for the <paramref name="assert"/> is false, then it returns a given error.
	/// </summary>
	/// <param name="assert">The delegate for the assertion</param>
	/// <param name="error">The error to return if the assertion is false</param>
	/// <returns>A new <see cref="Result{T,TError}"/> containing the value or the given error</returns>
	public Result<T, TError> Assert(Func<T, bool> assert, TError error)
	{
		if (IsError)
		{
			return InternalError;
		}

		return assert(InternalValue) ? InternalValue : error;
	}

	public static implicit operator Result<T, TError>(T value) => Result.Ok<T, TError>(value);
	public static implicit operator Result<T, TError>(TError error) => Result.Err<T, TError>(error);
}

public static class Result
{
	/// <summary>
	/// Creates a new instance of <see cref="Result{T,TError}"/> with a success state
	/// </summary>
	/// <param name="value">The non-null value for the success state</param>
	/// <typeparam name="T">The value type of the result</typeparam>
	/// <typeparam name="TError">The error type of the result</typeparam>
	/// <returns>A new <see cref="Result{T,TError}"/> with the given value</returns>
	public static Result<T, TError> Ok<T, TError>(T value) => new Ok<T, TError>(value);

	/// <summary>
	/// Creates a new instance of <see cref="Result{T,TError}"/> with a failure state
	/// </summary>
	/// <param name="error">The non-null error for the failure state</param>
	/// <typeparam name="T">The value type of the result</typeparam>
	/// <typeparam name="TError">The error type of the result</typeparam>
	/// <returns>A new <see cref="Result{T,TError}"/> with the given error</returns>
	public static Result<T, TError> Err<T, TError>(TError error) => new Err<T, TError>(error);
}