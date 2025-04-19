using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMessenger.API.Extensions;
using WebMessenger.Application.DTOs.Requests;
using WebMessenger.Application.UseCases.Interfaces;
using WebMessenger.Shared.Models;

namespace WebMessenger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
  [HttpPost("reg")]
  public async Task<IActionResult> Register([FromBody] RegisterRequest request)
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
    
    return this.ProcessError(result.Error);
  }

  [HttpPost("verify")]
  public async Task<IActionResult> VerifyEmail([FromQuery] string token)
  {
    var result = await authService.VerifyEmailAsync(token);
    
    if (result.IsSuccess)
      return Ok();
    
    return this.ProcessError(result.Error);
  }
  
  [HttpPost("resend-verify")]
  public async Task<IActionResult> ResendVerify([FromQuery] string email)
  {
    var result = await authService.ResendVerifyAsync(email);
    
    if (result.IsSuccess)
      return Ok();
    
    return this.ProcessError(result.Error);
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

    if (!result.IsSuccess)
      return this.ProcessError(result.Error);
    
    if (result.Data == null)
      throw new NullReferenceException(nameof(result.Data));
    
    if (result.Data.RefreshToken != null)
      Response.Cookies.Append("RefreshToken", result.Data.RefreshToken);
      
    return Ok(new AuthResult
    {
      Token = result.Data.AccessToken,
    });
  }
  
  [HttpGet("refresh-token")]
  public async Task<IActionResult> RefreshToken()
  {
    var token = Request.Cookies["RefreshToken"];
    if (string.IsNullOrEmpty(token))
      return Unauthorized();
    
    var result = await authService.RefreshTokenAsync(token);
    if (result.Data == null)
      throw new NullReferenceException(nameof(result.Data));
    
    if (result.IsSuccess)
    {
      return Ok(new AuthResult
      {
        Token = result.Data,
      });
    }
    
    return this.ProcessError(result.Error);
  }
  
  [Authorize]
  [HttpPost("revoke-token")]
  public async Task<IActionResult> RevokeToken()
  {
    var token = Request.Cookies["RefreshToken"];
    if (string.IsNullOrEmpty(token))
      return Unauthorized();
    
    var result = await authService.RevokeTokenAsync(token);
    Response.Cookies.Delete("RefreshToken");
    
    if (!result.IsSuccess)
      return this.ProcessError(result.Error);
    
    return Ok();
  }
  
  [HttpPost("forgot-password")]
  public async Task<IActionResult> ForgotPassword([FromQuery]string email)
  {
    var result = await authService.ForgotPasswordAsync(email);
    
    if (result.IsSuccess)
      return Ok();
    
    return this.ProcessError(result.Error);
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
    
    return this.ProcessError(result.Error);
  }
}

