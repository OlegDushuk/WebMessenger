using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMessenger.API.Extensions;
using WebMessenger.Application.UseCases.Interfaces;
using WebMessenger.Shared.DTOs.Requests;

namespace WebMessenger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
  [HttpPost("reg")]
  public async Task<IActionResult> Register([FromBody] RegisterDto request)
  {
    logger.LogInformation("Registration started with data:" +
                          "\n\tName: {name};\n\tUserName: {userName};" +
                          "\n\tEmail: {email};\n\tPassword: {password};", 
      request.Name, request.UserName, request.Email, request.Password);
    
    var result = await authService.RegisterAsync(request);

    if (!result.IsSuccess)
    {
      logger.LogError("Registration failed with error:\n\tType: {errorType};\n\tDetails: {errorDetails};",
        result.Error.Type, result.Error.Details);
      
      return this.ProcessError(result.Error);
    }

    logger.LogInformation("Registration with data: "+
                          "\n\tName: {name};\n\tUserName: {userName};" +
                          "\n\tEmail: {email};\n\tPassword: {password};\nis successful;",
      request.Name, request.UserName, request.Email, request.Password);
    
    return Ok(result.Data);
  }

  [HttpPost("verify")]
  public async Task<IActionResult> VerifyEmail([FromQuery] string token)
  {
    var result = await authService.ConfirmEmailAsync(token);
    
    if (result.IsSuccess)
      return Ok();
    
    return this.ProcessError(result.Error);
  }
  
  [HttpPost("resend-verify")]
  public async Task<IActionResult> ResendVerify([FromQuery] string email)
  {
    var result = await authService.ResendConfirmationAsync(email);
    
    if (result.IsSuccess)
      return Ok();
    
    return this.ProcessError(result.Error);
  }
  
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody]LoginDto request)
  {
    logger.LogInformation("Login started with data:\n\tEmail: {email};\n\tPassword: {password};",
      request.Email, request.Password);
    var result = await authService.LoginAsync(request);


    if (!result.IsSuccess)
    {
      logger.LogError("Login failed with error:\n\tType: {errorType};\n\tDetails: {errorDetails};",
        result.Error.Type, result.Error.Details);
      
      return this.ProcessError(result.Error);
    }
    
    if (result.Data == null)
      throw new NullReferenceException(nameof(result.Data));

    if (result.Data.RefreshToken != null)
    {
      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None,
        Expires = DateTimeOffset.UtcNow.AddDays(7),
        Path = "/"
      };
      
      Response.Cookies.Append("RefreshToken", result.Data.RefreshToken, cookieOptions);
    }
    
    logger.LogInformation("Login with data:\n\tEmail: {email};\n\tPassword: {password};\nis successful;",
      request.Email, request.Password);
    
    return Ok(result.Data);
  }
  
  [HttpGet("refresh-token")]
  public async Task<IActionResult> RefreshToken()
  {
    var token = Request.Cookies["RefreshToken"];
    if (string.IsNullOrEmpty(token))
      return Unauthorized();
    
    var result = await authService.RefreshAuthAsync(token);
    
    if (!result.IsSuccess)
      return this.ProcessError(result.Error);
    
    if (result.Data == null)
      throw new NullReferenceException(nameof(result.Data));
    
    return Ok(result.Data);
  }
  
  [Authorize]
  [HttpPost("revoke-token")]
  public async Task<IActionResult> RevokeToken()
  {
    var token = Request.Cookies["RefreshToken"];
    if (string.IsNullOrEmpty(token))
      return Unauthorized();
    
    var result = await authService.RevokeAuthAsync(token);
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
  public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
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

