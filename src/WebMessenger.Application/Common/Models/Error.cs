using WebMessenger.Application.Common.Enums;

namespace WebMessenger.Application.Common.Models;

public class Error(ErrorType type, object details)
{
  public ErrorType Type { get; } = type;
  public object Details { get; } = details;
  
  public static Error None => new(ErrorType.None, string.Empty);
}