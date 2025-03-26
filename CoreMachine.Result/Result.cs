// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
namespace CoreMachine.Result;


/// <summary>
/// Interface that represents a result with a success status
/// that can be used with pattern matching and deconstruction
/// to access the value
/// </summary>
/// <example>
/// <code>
/// var result = Result.Ok&lt;int, string&gt;(5);
/// if (result is IOk&lt;int&gt;(var num)) // variable in scope
/// {
///		return num + 5; // equals 10
/// }
/// var myNum = num; // the variable num is not assigned here, this will not compilee
/// </code>
/// </example>
/// <typeparam name="T">the type for the value contained in the <see cref="IOk{T}"/></typeparam>
public interface IOk<T>
{
	public T Value { get; }
	public void Deconstruct(out T value);
}

/// <summary>
/// Interface that represents a result with a failure status
/// that can be used with pattern matching and deconstruction
/// to access the error
/// </summary>
/// <example>
/// <code>
/// var result = Result.Err&lt;int, string&gt;("Error");
/// if (result is IErr&lt;string&gt;(var err)) // variable in scope
/// {
///		return err; // equals "Error"
/// }
/// var myErr = err; // the variable err is not assigned here, this will not compile
/// </code>
/// </example>
/// <typeparam name="TE">the type for the error contained in the <see cref="IErr{TE}"/></typeparam>
public interface IErr<TE>
{
	public TE Error { get; }
	public void Deconstruct(out TE error);
}

/// <summary>
///   Result type that represents the union between <see cref="T" />
///   and <see cref="TE" /> with a bunch of methods for error handling
/// </summary>
/// <typeparam name="T">The type of the success path</typeparam>
/// <typeparam name="TE">The type of the error path</typeparam>
public abstract record Result<T, TE>
{
	private record Ok(T Value) : Result<T, TE>, IOk<T>;

	private record Err(TE Error) : Result<T, TE>, IErr<TE>;
	
	public static implicit operator Result<T, TE>(T value) => new Ok(value);
	public static implicit operator Result<T, TE>(TE error) => new Err(error);

	/// <summary>
	///   Matches the <see cref="Result{T,TE}" /> state and executes the corresponding delegate,
	///   mapping both possibilities into a single type
	/// </summary>
	/// <param name="ok">The function for the success path</param>
	/// <param name="err">The function for the failure path</param>
	/// <typeparam name="TOut">The mapped result type</typeparam>
	/// <returns>The result of either delegate</returns>
	public TOut Match<TOut>(Func<T, TOut> ok, Func<TE, TOut> err) =>
		this switch
		{
			Ok(var value) => ok(value), 
			Err(var error) => err(error),
			_ => throw new InvalidOperationException()
		};

	/// <summary>
	///   Matches the <see cref="Result{T,TE}" /> state and executes
	///   the corresponding asynchronous delegate,
	///   mapping both possibilities into a single type
	/// </summary>
	/// <param name="ok">The asynchronous function for the success path</param>
	/// <param name="err">The asynchronous function for the failure path</param>
	/// <typeparam name="TOut">The mapped result type</typeparam>
	/// <returns>the <see cref="Task{TResult}" /> containing the result of either delegate</returns>
	public Task<TOut> MatchAsync<TOut>(Func<T, Task<TOut>> ok, Func<TE, Task<TOut>> err) =>
		this switch
		{
			Ok(var value) => ok(value), 
			Err(var error) => err(error),
			_ => throw new InvalidOperationException()
		};

	/// <summary>
	///   Matches the <see cref="Result{T,TE}" /> state and executes
	///   the corresponding action
	/// </summary>
	/// <param name="ok">The action for the success path</param>
	/// <param name="err">The action for the failure path</param>
	public void Switch(Action<T> ok, Action<TE> err)
	{
		switch (this)
		{
			case Ok(var value):
				ok(value);
				break;
			case Err(var error):
				err(error);
				break;
		}
	}

	/// <summary>
	///   Matches the <see cref="Result{T,TE}" /> state and executes
	///   the corresponding asynchronous action
	/// </summary>
	/// <param name="ok">The asynchronous action for the success path</param>
	/// <param name="err">The asynchronous action for the failure path</param>
	public Task SwitchAsync(Func<T, Task> ok, Func<TE, Task> err) =>
		this switch
		{
			Ok(var value) => ok(value),
			Err(var error) => err(error),
			_ => throw new InvalidOperationException()
		};


	/// <summary>
	///   Maps the <see cref="T" /> into a new type,
	///   leaving the <see cref="TE" /> type intact
	/// </summary>
	/// <param name="next">The type mapper function</param>
	/// <typeparam name="TNew">The new value type</typeparam>
	/// <returns>A new <see cref="Result{T,TNew}" /> with <see cref="TNew" /> as the new value type</returns>
	public Result<TNew, TE> Map<TNew>(Func<T, TNew> next)
		=> this switch
		{
			Ok(var value) => next(value),
			Err(var error) => error,
			_ => throw new InvalidOperationException()
		};

	/// <summary>
	///   Maps the <see cref="T" /> into a new type asynchronously,
	///   leaving the <see cref="TE" /> type intact
	/// </summary>
	/// <param name="next">The asynchronous type mapper function</param>
	/// <param name="continueOnCapturedContext">
	/// true to attempt to marshal the continuation back to the original context captured; otherwise, false
	/// </param>
	/// <typeparam name="TNew">The new value type</typeparam>
	/// <returns>
	///   A <see cref="Task{T}" /> containing a new <see cref="Result{T,TNew}" /> with <see cref="TNew" />
	///   as the new value type
	/// </returns>
	public async Task<Result<TNew, TE>> MapAsync<TNew>(Func<T, Task<TNew>> next, bool continueOnCapturedContext = false)
		=> this switch
		{
			Ok(var value) => await next(value).ConfigureAwait(continueOnCapturedContext),
			Err(var error) => error,
			_ => throw new InvalidOperationException()
		};

	/// <summary>
	///   Maps the <see cref="TE" /> into a new type,
	///   leaving the <see cref="T" /> type intact
	/// </summary>
	/// <param name="next">The error type mapper function</param>
	/// <typeparam name="TNew">The new error type</typeparam>
	/// <returns>A new <see cref="Result{T,TNew}" /> with <see cref="TNew" /> as the new error type</returns>
	public Result<T, TNew> MapError<TNew>(Func<TE, TNew> next)
		=> this switch
		{
			Ok(var value) => value,
			Err(var error) => next(error),
			_ => throw new InvalidOperationException()
		};

	/// <summary>
	///   Maps the <see cref="TE" /> into a new type asynchronously,
	///   leaving the <see cref="T" /> type intact
	/// </summary>
	/// <param name="next">The asynchronous error type mapper function</param>
	/// <param name="continueOnCapturedContext">
	/// true to attempt to marshal the continuation back to the original context captured; otherwise, false.
	/// </param>
	/// <typeparam name="TNew">The new error type</typeparam>
	/// <returns>
	///   A <see cref="Task{T}" /> containing a new <see cref="Result{T,TNew}" /> with <see cref="TNew" />
	///   as the new error type
	/// </returns>
	public async Task<Result<T, TNew>> MapErrorAsync<TNew>(Func<TE, Task<TNew>> next,
		bool continueOnCapturedContext = false)
		=> this switch
		{
			Ok(var value) => value,
			Err(var error) => await next(error).ConfigureAwait(continueOnCapturedContext),
			_ => throw new InvalidOperationException()
		};

	/// <summary>
	///   Binds another function returning <see cref="Result{T,TE}" />
	///   with <see cref="TNew" /> as the new value type.
	///   The binded function only executes if the <see cref="Result{T,TE}" />
	///   state is success, otherwise it returns the current error
	/// </summary>
	/// <param name="next">Binded function</param>
	/// <typeparam name="TNew">The new value type for the <see cref="Result{T,TE}" /></typeparam>
	/// <returns>A new <see cref="Result{T,TE}" /> with the new value type</returns>
	public Result<TNew, TE> Bind<TNew>(Func<T, Result<TNew, TE>> next) =>
		this switch
		{
			Ok(var value) => next(value),
			Err(var error) => error,
			_ => throw new InvalidOperationException()
		};

	/// <summary>
	///   Binds another asynchronous function returning <see cref="Result{T,TE}" />
	///   with <see cref="TNew" /> as the new value type.
	///   The binded asynchronous function only executes if the <see cref="Result{T,TE}" />
	///   state is success, otherwise it returns the current error
	/// </summary>
	/// <param name="next">Binded asynchronous function</param>
	/// <typeparam name="TNew">The new value type for the <see cref="Result{T,TE}" /></typeparam>
	/// <returns>
	///   A <see cref="Task{TResult}" /> containing a new <see cref="Result{T,TE}" /> with the new value type
	/// </returns>
	public Task<Result<TNew, TE>> BindAsync<TNew>(Func<T, Task<Result<TNew, TE>>> next) =>
		this switch
		{
			Ok(var value) => next(value),
			Err(var error) => Task.FromResult<Result<TNew, TE>>(error),
			_ => throw new InvalidOperationException()
		};

	/// <summary>
	///   Binds another function returning <see cref="Result{T,TE}" />
	///   with <see cref="TNewError" /> as the new error type.
	///   The binded function only executes if the <see cref="Result{T,TE}" />
	///   state is success, otherwise it returns the current error
	/// </summary>
	/// <param name="next">Binded function</param>
	/// <typeparam name="TNewError">The new error type for the <see cref="Result{T,TE}" /></typeparam>
	/// <returns>A new <see cref="Result{T,TE}" /> with the new error type</returns>
	public Result<T, TNewError> BindError<TNewError>(Func<TE, Result<T, TNewError>> next)
		=> this switch
		{
			Ok(var value) => value,
			Err(var error) => next(error),
			_ => throw new InvalidOperationException()
		};

	/// <summary>
	///   Binds another asynchronous function returning <see cref="Result{T,TE}"/>
	///   with <see cref="TNewError" /> as the new error type.
	///   The binded asynchronous function only executes if the <see cref="Result{T,TE}" />
	///   state is success, otherwise it returns the current error
	/// </summary>
	/// <param name="next">Binded asynchronous function</param>
	/// <typeparam name="TNewError">The new error type for the <see cref="Result{T,TE}" /></typeparam>
	/// <returns>
	///   A <see cref="Task{TResult}" /> containing a new <see cref="Result{T,TE}" /> with the new error type
	/// </returns>
	public Task<Result<T, TNewError>> BindErrorAsync<TNewError>(
		Func<TE, Task<Result<T, TNewError>>> next)
		=> this switch
		{
			Ok(var value) => Task.FromResult<Result<T, TNewError>>(value),
			Err(var error) => next(error),
			_ => throw new InvalidOperationException()
		};

	/// <summary>
	///   Executes a given delegate-like function if the <see cref="Result{T,TE}" /> state is success,
	///   if the result for the <paramref name="assert" /> is false, then it returns a given error.
	/// </summary>
	/// <param name="assert">The delegate for the assertion</param>
	/// <param name="error">The error to return if the assertion is false</param>
	/// <returns>A new <see cref="Result{T,TE}" /> containing the value or the given error</returns>
	public Result<T, TE> Assert(Func<T, bool> assert, TE error) =>
		this switch
		{
			Ok(var value) => assert(value) ? value : error,
			Err(var previousError) => previousError,
			_ => throw new InvalidOperationException()
		};
}

public static class Result
{
	/// <summary>
	///   Creates a new instance of <see cref="Result{T,TE}" /> with a success state
	/// </summary>
	/// <param name="value">The value for the success state</param>
	/// <typeparam name="T">The value type of the result</typeparam>
	/// <typeparam name="TE">The error type of the result</typeparam>
	/// <returns>A new <see cref="Result{T,TE}" /> with the given value</returns>
	public static Result<T, TE> Ok<T, TE>(T value) => value;

	/// <summary>
	///   Creates a new instance of <see cref="Result{T,TE}" /> with a failure state
	/// </summary>
	/// <param name="error">The error for the failure state</param>
	/// <typeparam name="T">The value type of the result</typeparam>
	/// <typeparam name="TE">The error type of the result</typeparam>
	/// <returns>A new <see cref="Result{T,TE}" /> with the given error</returns>
	public static Result<T, TE> Err<T, TE>(TE error) => error;
}