using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMessenger.API.Extensions;
using WebMessenger.Application.UseCases.Interfaces;
using WebMessenger.Shared.DTOs.Requests;

namespace WebMessenger.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController(IChatService chatService) : ControllerBase
{
  [HttpGet("chats")]
  public async Task<IActionResult> GetChats()
  {
    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
    
    var result = await chatService.GetChats(userEmail);
    
    if (!result.IsSuccess)
      return this.ProcessError(result.Error);
    
    if (result.Data == null)
      throw new NullReferenceException(nameof(result.Data));
    
    return Ok(result.Data);
  }
  
  [HttpGet("history/{chatId:guid}/{page:int}/{pageSize:int}")]
  public async Task<IActionResult> GetChatHistory(Guid chatId, int page, int pageSize)
  {
    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
    
    var result = await chatService.GetChatHistory(userEmail, chatId, page, pageSize);
    
    if (!result.IsSuccess)
      return this.ProcessError(result.Error);
    
    if (result.Data == null)
      throw new NullReferenceException(nameof(result.Data));
    
    return Ok(result.Data);
  }
  
  [HttpGet("members/{chatId:guid}")]
  public async Task<IActionResult> GetChatMembers(Guid chatId)
  {
    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
    
    var result = await chatService.GetChatMembers(userEmail, chatId);
    
    if (!result.IsSuccess)
      return this.ProcessError(result.Error);
    
    if (result.Data == null)
      throw new NullReferenceException(nameof(result.Data));
    
    return Ok(result.Data);
  }
  
  [HttpPost("group")]
  public async Task<IActionResult> CreateGroup([FromBody] CreateGroupChatDto dto)
  {
    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
    
    var result = await chatService.CreateGroupChat(userEmail, dto);

    if (!result.IsSuccess)
      return this.ProcessError(result.Error);
    
    if (result.Data == null)
      throw new NullReferenceException(nameof(result.Data));
    
    return Ok(result.Data);
  }
  
  [HttpPost("send")]
  public async Task<IActionResult> SendMessage([FromBody] SendMessageDto dto)
  {
    var result = await chatService.SendMessage(dto);
    
    if (!result.IsSuccess)
      return this.ProcessError(result.Error);
    
    if (result.Data == null)
      throw new NullReferenceException(nameof(result.Data));
    
    return Ok(result.Data);
  }
  
  [HttpPost("private")]
  public async Task<IActionResult> CreatePrivateChat([FromQuery] string otherUserName)
  {
    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
    
    var result = await chatService.CreatePrivateChat(userEmail, otherUserName);
    
    return Ok(result);
  }
}