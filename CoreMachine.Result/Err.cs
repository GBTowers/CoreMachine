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
        ProtectedError = error;
        IsError = true;
    }
    
    private protected override bool IsError { get; }
    public TError Error => ProtectedError;
    private protected override TError ProtectedError { get; }
    private protected override T ProtectedValue => throw new InvalidOperationException();
    public static implicit operator TError(Err<T, TError> val) => val.ProtectedError;
}