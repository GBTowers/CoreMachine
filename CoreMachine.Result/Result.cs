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

    public TOut Match<TOut>(Func<TValue, TOut> ok, Func<TError, TOut> err)
        => IsError ? err(_error) : ok(_value);

    public async Task<TOut> MatchAsync<TOut>(Func<TValue, Task<TOut>> ok, Func<TError, Task<TOut>> err)
        => IsError ? await err(_error).ConfigureAwait(false) : await ok(_value).ConfigureAwait(false);

    public void Switch(Action<TValue> ok, Action<TError> err)
    {
        if (IsError)
        {
            err(_error);
            return;
        }

        ok(_value);
    }

    public async Task SwitchAsync(Func<TValue, Task> ok, Func<TError, Task> err)
    {
        if (IsError)
        {
            await err(_error);
            return;
        }

        await ok(_value);
    }

    public Result<TValue, TNew> MapError<TNew>(Func<TError, TNew> next) 
        => IsError ? next(_error) : _value;

    public async Task<Result<TValue, TNew>> MapErrorAsync<TNew>(Func<TError, Task<TNew>> next) 
        => IsError ? await next(_error).ConfigureAwait(false) : _value;
    
    public Result<TNew, TError> Map<TNew>(Func<TValue, TNew> next) 
        => !IsError ? next(_value) : _error;

    public async Task<Result<TNew, TError>> MapAsync<TNew>(Func<TValue, Task<TNew>> next)
        => !IsError ? await next(_value).ConfigureAwait(false) : _error;

    public Result<TNew, TError> Bind<TNew>(Func<TValue, Result<TNew, TError>> next) 
        => !IsError ? next(_value) : _error; 
    
    public async Task<Result<TNew, TError>> BindAsync<TNew>(Func<TValue, Task<Result<TNew, TError>>> next) 
        => !IsError ? await next(_value).ConfigureAwait(false) : _error;
    
    public Result<TValue, TError> Assert(Func<TValue, bool> assert, TError error)
    {
        if (IsError)
        {
            return _error;
        }
        
        return assert(_value) ? _value : error;
    }
    
    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);
}

public static class Result
{
    public static Result<T, TE> Ok<T, TE>(T value) => value;
    public static Result<T, TE> Error<T, TE>(TE error) => error;
}