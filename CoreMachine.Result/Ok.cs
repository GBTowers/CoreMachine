namespace CoreMachine.Result;

public interface IOk<out T>
{
	public T Value { get; }
}

/// <inheritdoc cref="Result{T,TError}" />
public sealed record Ok<T, TError> : Result<T, TError>, IOk<T>
{
	public Ok(T value)
	{
		ProtectedValue = value;
		IsError = false;
	}

	private protected override bool IsError { get; }
	private protected override T ProtectedValue { get; }
	private protected override TError ProtectedError => throw new InvalidOperationException();
	public T Value => ProtectedValue;

	public static implicit operator T(Ok<T, TError> val)
	{
		return val.Value;
	}
}