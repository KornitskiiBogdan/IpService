using System.Text.Json.Serialization;

namespace IpService.Domain.Entities;

public interface IResult
{
    public Error? Error { get; }

    public bool IsSuccess { get; }
}

public interface IResult<out T> : IResult
{
    public T? Value { get; }
}

public readonly struct Result<TValue> : IResult<TValue>
{
    [JsonConstructor]
    private Result(TValue? value, Error? error, bool isSuccess)
    {
        Value = value;
        Error = error;
        IsSuccess = isSuccess;
    }

    public TValue? Value { get; }

    public Error? Error { get; }

    public bool IsSuccess { get; }

    public static Result<TValue> Success(TValue v) => new(v, null, true);

    public static Result<TValue> Failure(Error e) => new(default, e, false);

    public static implicit operator Result<TValue>(TValue v) => Success(v);
    public static implicit operator Result<TValue>(Error e) => Failure(e);
}

public readonly struct Result : IResult
{
    [JsonConstructor]
    private Result(Error? error, bool isSuccess)
    {
        Error = error;
        IsSuccess = isSuccess;
    }

    public Error? Error { get; }

    public bool IsSuccess { get; }

    public static Result Success() => new(null, true);

    public static Result Failure(Error e) => new(e, false);

    public static implicit operator Result(Error e) => Failure(e);
}