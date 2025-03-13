namespace CoreMachine.Result;

public record Err<T, TError> : Result<T, TError>
{
    public Err(TError error) : base(error)
    {
        Error = error;
    }

    public TError Error { get; }

    public static implicit operator TError(Err<T, TError> val) => val.Error;
}