using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMessenger.API.Extensions;
using WebMessenger.Application.UseCases.Interfaces;

namespace WebMessenger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SearchController(ISearchService searchService) : ControllerBase
{
  [HttpGet("user")]
  public async Task<IActionResult> SearchUsers([FromQuery] string searchRequest)
  {
    var result = await searchService.FindUsers(searchRequest);
    
    if (!result.IsSuccess)
      return this.ProcessError(result.Error);
    
    return Ok(result.Data);
  }
  
  [HttpGet("chat")]
  public async Task<IActionResult> SearchGroups([FromQuery] string searchRequest)
  {
    var result = await searchService.FindChats(searchRequest);
    
    if (!result.IsSuccess)
      return this.ProcessError(result.Error);
    
    return Ok(result.Data);
  }
}