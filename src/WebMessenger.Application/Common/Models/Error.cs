using WebMessenger.Application.Common.Enums;

namespace WebMessenger.Application.Common.Models;

public class Error(ErrorType type, object? info)
{
  public ErrorType Type { get; } = type;
  public object? Info { get; } = info;
  
  public static Error None => new(ErrorType.None, null);
}