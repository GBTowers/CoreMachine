namespace CoreMachine.Result;

public interface IErr<out T>
{
    public T Error { get; }
}
/// <inheritdoc cref="Result{T,TError}"/>
public record Err<T, TError> : Result<T, TError>, IErr<TError>
{
    public Err(TError error) : base(error)
    {
        Error = error;
    }

    public TError Error { get; }

    public static implicit operator TError(Err<T, TError> val) => val.Error;
}