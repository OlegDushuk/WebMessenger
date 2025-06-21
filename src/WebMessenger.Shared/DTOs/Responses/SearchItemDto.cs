using WebMessenger.Shared.Enums;

namespace WebMessenger.Shared.DTOs.Responses;

public class SearchItemDto
{
  public ChatDto? Chat { get; set; }
  public UserDto? User { get; set; }
  
  public SearchItemTypeDto Type { get; set; }
}