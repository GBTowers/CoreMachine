using System.Diagnostics.CodeAnalysis;

namespace CoreMachine.Result;

public readonly record struct Result<TValue, TError>
{
    private readonly TError? _error;
    private readonly TValue? _value;
    
    [MemberNotNullWhen(true, nameof(_error))]
    [MemberNotNullWhen(false, nameof(_value))]
    private bool IsError { get; }

    public Result(TValue value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        _value = value;
        _error = default;
        IsError = false;
    }

    public Result(TError error)
    {
        if (error is null)
        {
            throw new ArgumentNullException(nameof(error));
        }
        _error = error;
        _value = default;
        IsError = true;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);
}

