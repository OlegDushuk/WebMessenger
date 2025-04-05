using WebMessenger.Application.Common.Enums;

namespace WebMessenger.Application.Common.Models;

public class Result
{
  public bool IsSuccess { get; init; }
  public Error Error { get; init; } = Error.None;
  
  public static Result Success => new() { IsSuccess = true };
  public static Result Failure(ErrorType errorType, object? errorInfo) => new()
  {
    IsSuccess = false,
    Error = new Error(errorType, errorInfo),
  };
}

public class Result<T> : Result
{
  public T? Data { get; init; }
  
  public new static Result<T> Success(T data) => new()
  {
    IsSuccess = true,
    Data = data,
  };
  public new static Result<T> Failure(ErrorType errorType, object? errorInfo) => new()
  {
    IsSuccess = false,
    Error = new Error(errorType, errorInfo),
  };
}
