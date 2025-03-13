using System.Diagnostics.CodeAnalysis;
// ReSharper disable UnusedMember.Global

namespace CoreMachine.Result;

public record Result<T, TError>
{
    private readonly TError? _error;
    private readonly T? _value;

    [MemberNotNullWhen(true, nameof(_error))]
    [MemberNotNullWhen(false, nameof(_value))]
    private bool IsError { get; }
    
    protected Result(T value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        _value = value;
        _error = default;
        IsError = false;
    }

    protected Result(TError error)
    {
        if (error is null)
        {
            throw new ArgumentNullException(nameof(error));
        }

        _error = error;
        _value = default;
        IsError = true;
    }

    public TOut Match<TOut>(Func<T, TOut> ok, Func<TError, TOut> err)
        => IsError ? err(_error) : ok(_value);

    public async Task<TOut> MatchAsync<TOut>(Func<T, Task<TOut>> ok, Func<TError, Task<TOut>> err)
        => IsError ? await err(_error).ConfigureAwait(false) : await ok(_value).ConfigureAwait(false);

    public void Switch(Action<T> ok, Action<TError> err)
    {
        if (IsError)
        {
            err(_error);
            return;
        }

        ok(_value);
    }

    public async Task SwitchAsync(Func<T, Task> ok, Func<TError, Task> err)
    {
        if (IsError)
        {
            await err(_error);
            return;
        }

        await ok(_value);
    }

    public Result<T, TNew> MapError<TNew>(Func<TError, TNew> next) 
        => IsError ? next(_error) : _value;

    public async Task<Result<T, TNew>> MapErrorAsync<TNew>(Func<TError, Task<TNew>> next) 
        => IsError ? await next(_error).ConfigureAwait(false) : _value;
    
    public Result<TNew, TError> Map<TNew>(Func<T, TNew> next) 
        => !IsError ? next(_value) : _error;

    public async Task<Result<TNew, TError>> MapAsync<TNew>(Func<T, Task<TNew>> next)
        => !IsError ? await next(_value).ConfigureAwait(false) : _error;

    public Result<TNew, TError> Bind<TNew>(Func<T, Result<TNew, TError>> next) 
        => !IsError ? next(_value) : _error; 
    
    public async Task<Result<TNew, TError>> BindAsync<TNew>(Func<T, Task<Result<TNew, TError>>> next) 
        => !IsError ? await next(_value).ConfigureAwait(false) : _error;
    
    public Result<T, TNewError> BindError<TNewError>(Func<TError, Result<T, TNewError>> next) 
        => IsError ? next(_error) : _value; 
    
    public async Task<Result<T, TNewError>> BindErrorAsync<TNewError>(
        Func<TError, Task<Result<T, TNewError>>> next) 
        => IsError ? await next(_error).ConfigureAwait(false) : _value;
    public Result<T, TError> Assert(Func<T, bool> assert, TError error)
    {
        if (IsError)
        {
            return _error;
        }
        
        return assert(_value) ? _value : error;
    }
    
    public static implicit operator Result<T, TError>(T value) => new Ok<T, TError>(value);
    public static implicit operator Result<T, TError>(TError error) => new Err<T, TError>(error);
}

public static class Result
{
    public static Result<T, TError> Ok<T, TError>(T value) => value;
    public static Result<T, TError> Error<T, TError>(TError error) => error;
}