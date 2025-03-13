namespace CoreMachine.Result;

public record Ok<T, TError> : Result<T, TError>
{
    public Ok(T value) : base(value)
    {
        Value = value;
    }

    public T Value { get; }
    
    public static implicit operator T(Ok<T, TError> val) => val.Value;
}