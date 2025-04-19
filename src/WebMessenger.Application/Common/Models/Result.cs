using WebMessenger.Application.Common.Enums;

namespace WebMessenger.Application.Common.Models;

public class Result
{
  public bool IsSuccess { get; protected init; }
  public Error Error { get; protected init; } = Error.None;
  
  public static Result Success => new() { IsSuccess = true };
  public static Result Failure(ErrorType type, object details) => new()
  {
    IsSuccess = false,
    Error = new Error(type, details),
  };
}

public class Result<T> : Result
{
  public T? Data { get; private init; }
  
  public new static Result<T> Success(T data) => new()
  {
    IsSuccess = true,
    Data = data,
  };
  public new static Result<T> Failure(ErrorType type, object details) => new()
  {
    IsSuccess = false,
    Error = new Error(type, details),
  };
}
