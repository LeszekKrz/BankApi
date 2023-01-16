namespace BankAPI.Results;

public sealed class Result<T>
{
    private readonly T _value;

    public Result(T value)
    {
        _value = value;
        HasValue = true;
    }

    public Result(ResultException exception)
    {
        Exception = exception;
        HasValue = false;
        _value = default!;
    }

    public bool HasValue { get; }

    public T Value => HasValue
        ? _value
        : throw (Exception as Exception
                 ?? new InvalidStateException("Result", "result does not contain an exception, but HasValue is false"));

    public ResultException? Exception { get; }

    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    public static implicit operator Result<T>(ResultException exception)
    {
        return new Result<T>(exception);
    }
}