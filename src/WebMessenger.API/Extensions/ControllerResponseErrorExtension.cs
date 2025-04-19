using Microsoft.AspNetCore.Mvc;
using WebMessenger.Application.Common.Enums;
using WebMessenger.Application.Common.Models;

namespace WebMessenger.API.Extensions;

public static class ControllerResponseErrorExtension
{
  public static IActionResult ProcessError(this ControllerBase controller, Error error)
  {
    return controller.StatusCode(
      ErrorTypeToStatusCode(error.Type),
      error.Details
    );
  }
  
  private static int ErrorTypeToStatusCode(ErrorType errorType)
  {
    return errorType switch
    {
      ErrorType.None => 200,
      ErrorType.Validation => 422,
      ErrorType.NotFound => 404,
      ErrorType.Conflict => 409,
      ErrorType.Unauthorized => 401,
      ErrorType.Forbidden => 403,
      ErrorType.Internal => 500,
      _ => throw new ArgumentOutOfRangeException(nameof(errorType), errorType, null)
    };
  }
}