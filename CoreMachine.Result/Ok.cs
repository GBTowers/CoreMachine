namespace CoreMachine.Result;

public interface IOk<out T>
{
	public T Value { get; }
}

/// <inheritdoc cref="Result{T,TError}"/>
public sealed record Ok<T, TError> : Result<T, TError>, IOk<T>
{
	public Ok(T value)
	{
		if (value is null)
			throw new ArgumentNullException(nameof(value),
				"Cannot receive null value as parameter, use ValueOr<T, TE> extension method instead");
		InternalValue = value;
	}

	public T Value => InternalValue;

	private protected override T InternalValue { get; }
	private protected override TError InternalError => throw new InvalidOperationException("This cannot be done");
	private protected override bool IsError => false;
	public static implicit operator T(Ok<T, TError> val) => val.Value;
}