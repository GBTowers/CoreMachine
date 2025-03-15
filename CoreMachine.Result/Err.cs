namespace CoreMachine.Result;

public interface IErr<out T>
{
	public T Error { get; }
}

/// <inheritdoc cref="Result{T,TError}"/>
public sealed record Err<T, TError> : Result<T, TError>, IErr<TError>
{
	public Err(TError error)
	{
		if (error is null)
			throw new ArgumentNullException(nameof(error),
				"Cannot receive null value as parameter, use ValueOr<T, TE> extension method instead");
		InternalError = error;
	}

	public TError Error => InternalError;

	private protected override T InternalValue => throw new InvalidOperationException("This cannot be done");
	private protected override TError InternalError { get; }
	private protected override bool IsError => true;
	public static implicit operator TError(Err<T, TError> val) => val.Error;
}