using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMessenger.API.Extensions;
using WebMessenger.Application.UseCases.Interfaces;
using WebMessenger.Shared.DTOs.Requests;

namespace WebMessenger.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService accountService) : ControllerBase
{
  [HttpGet("user")]
  public async Task<IActionResult> GetUser()
  {
    var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
    var result = await accountService.GetUserAsync(email);

    if (!result.IsSuccess)
      return this.ProcessError(result.Error);
    
    if (result.Data == null)
      throw new NullReferenceException(nameof(result.Data));
    
    return Ok(result.Data);
  }

  [HttpPut("update-data")]
  public async Task<IActionResult> UpdateAccountData(UpdateAccountDataDto request)
  {
    var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
    
    var result = await accountService.UpdateAccountDataAsync(email, new UpdateAccountDataDto
    {
      UserName = request.UserName,
      Name = request.Name,
      Bio = request.Bio,
    });
    
    if (result.IsSuccess)
      return Ok();
      
    return this.ProcessError(result.Error);
  }

  [HttpPut("change-password")]
  public async Task<IActionResult> ChangePassword(ChangePasswordDto request)
  {
    var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
    
    var result = await accountService.ChangePasswordAsync(email, new ChangePasswordDto
    {
      OldPassword = request.OldPassword,
      NewPassword = request.NewPassword,
    });
    
    if (result.IsSuccess)
      return Ok();
      
    return this.ProcessError(result.Error);
  }


  [HttpDelete("delete")]
  public async Task<IActionResult> Delete()
  {
    var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
    var result = await accountService.DeleteUserAsync(email);
      
    if (result.IsSuccess)
      return Ok();
      
    return this.ProcessError(result.Error);
  }
}