namespace CoreMachine.Result;

public interface IOk<out T>
{
    public T Value { get; }
}
/// <inheritdoc cref="Result{T,TError}"/>
public record Ok<T, TError> : Result<T, TError>, IOk<T>
{
    public Ok(T value) : base(value)
    {
        Value = value;
    }

    public T Value { get; }
    
    public static implicit operator T(Ok<T, TError> val) => val.Value;
}