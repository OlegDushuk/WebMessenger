using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMessenger.API.Models.Requests;
using WebMessenger.Application.Common.Enums;
using WebMessenger.Application.DTOs.Requests;
using WebMessenger.Application.UseCases.Interfaces;

namespace WebMessenger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
  [HttpPost("reg")]
  public async Task<IActionResult> Register([FromBody]RegisterRequest request)
  {
    var dto = new RegisterDto
    {
      Email = request.Email,
      Password = request.Password,
      UserName = request.UserName,
      Name = request.Name
    };

    var result = await authService.RegisterAsync(dto);
    
    if (result.IsSuccess)
      return Ok();
    
    return result.Error.Type switch
    {
      ErrorType.Validation => BadRequest(result.Error.Info),
      ErrorType.Conflict => Conflict(result.Error.Info),
      _ => BadRequest(result.Error.Info)
    };
  }
  
  [HttpPost("verify")]
  public async Task<IActionResult> VerifyEmail([FromQuery] string token)
  {
    var result = await authService.VerifyEmailAsync(token);
    
    if (result.IsSuccess)
      return Ok();
    
    return result.Error.Type switch
    {
      ErrorType.NotFound => NotFound(result.Error.Info),
      ErrorType.Conflict => Conflict(result.Error.Info),
      _ => BadRequest(result.Error.Info)
    };
  }
  
  [HttpPost("resend-verify")]
  public async Task<IActionResult> ResendVerify([FromQuery] string email)
  {
    var result = await authService.ResendVerifyAsync(email);
    
    if (result.IsSuccess)
      return Ok();
    
    return result.Error.Type switch
    {
      ErrorType.Validation => BadRequest(result.Error.Info),
      ErrorType.NotFound => NotFound(result.Error.Info),
      ErrorType.Conflict => Conflict(result.Error.Info),
      _ => BadRequest(result.Error.Info)
    };
  }
  
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody]LoginRequest request)
  {
    var result = await authService.LoginAsync(new LoginDto
    {
      Email = request.Email,
      Password = request.Password,
      RememberMe = request.RememberMe
    });
    
    if (result.IsSuccess)
    {
      if (result.Data!.RefreshToken != null)
        Response.Cookies.Append("RefreshToken", result.Data!.RefreshToken!);
      return Ok(new
      {
        result.Data!.AccessToken, result.Data!.Expires,
      });
    }
    
    return result.Error.Type switch
    {
      ErrorType.Validation => BadRequest(result.Error.Info),
      ErrorType.NotFound => NotFound(result.Error.Info),
      ErrorType.Conflict => Conflict(result.Error.Info),
      _ => BadRequest(result.Error.Info)
    };
  }
  
  [HttpPost("refresh-token")]
  public async Task<IActionResult> RefreshToken()
  {
    var token = Request.Cookies["RefreshToken"];
    if (string.IsNullOrEmpty(token))
      return Unauthorized();
    
    var result = await authService.RefreshTokenAsync(token);
    
    if (result.IsSuccess)
    {
      return Ok(new
      {
        AccessToken = result.Data.Item1,
        Expires = result.Data.Item2,
      });
    }
    
    return result.Error.Type switch
    {
      ErrorType.Validation => BadRequest(result.Error.Info),
      ErrorType.NotFound => NotFound(result.Error.Info),
      ErrorType.Conflict => Conflict(result.Error.Info),
      _ => BadRequest(result.Error.Info)
    };
  }
  
  [Authorize]
  [HttpPost("revoke-token")]
  public async Task<IActionResult> RevokeToken()
  {
    var token = Request.Cookies["RefreshToken"];
    if (string.IsNullOrEmpty(token))
      return Unauthorized();

    var result = await authService.RevokeTokenAsync(token);
    
    if (result.IsSuccess)
      return Ok();
    
    return result.Error.Type switch
    {
      ErrorType.NotFound => NotFound(result.Error.Info),
      _ => BadRequest(result.Error.Info)
    };
  }
  
  [HttpPost("forgot-password")]
  public async Task<IActionResult> ForgotPassword([FromQuery]string email)
  {
    var result = await authService.ForgotPasswordAsync(email);
    
    if (result.IsSuccess)
      return Ok();
    
    return result.Error.Type switch
    {
      ErrorType.Validation => BadRequest(result.Error.Info),
      ErrorType.NotFound => NotFound(result.Error.Info),
      _ => BadRequest(result.Error.Info)
    };
  }
  
  [HttpPost("reset-password")]
  public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
  {
    var result = await authService.ResetPasswordAsync(new ResetPasswordDto
    {
      Token = request.Token,
      NewPassword = request.NewPassword,
    });
    
    if (result.IsSuccess)
      return Ok();
    
    return result.Error.Type switch
    {
      ErrorType.Validation => BadRequest(result.Error.Info),
      ErrorType.NotFound => NotFound(result.Error.Info),
      _ => BadRequest(result.Error.Info)
    };
  }
  
  [Authorize]
  [HttpGet("user")]
  public async Task<IActionResult> GetUser()
  {
    var email = User.FindFirst(ClaimTypes.Email)!.Value;
    var result = await authService.GetUserAsync(email);
    
    if (result.IsSuccess)
      return Ok(new { User = result.Data! });
    
    return result.Error.Type switch
    {
      ErrorType.Validation => BadRequest(result.Error.Info),
      ErrorType.NotFound => NotFound(result.Error.Info),
      _ => BadRequest(result.Error.Info)
    };
  }
  
  [Authorize]
  [HttpGet("delete")]
  public async Task<IActionResult> Delete()
  {
    var email = User.FindFirst(ClaimTypes.Email)!.Value;
    var result = await authService.DeleteUserAsync(email);
      
      if (result.IsSuccess)
        return Ok();
      
    return result.Error.Type switch
    {
      ErrorType.Validation => BadRequest(result.Error.Info),
      ErrorType.NotFound => NotFound(result.Error.Info),
      _ => BadRequest(result.Error.Info)
    };
  }
}