using WebMessenger.Shared.Enums;

namespace WebMessenger.Shared.DTOs.Requests;

public class CreateGroupChatDto
{
  public string Name { get; set; } = string.Empty;
  public string? Bio { get; set; }
  public string? Avatar { get; set; }
  public GroupTypeDto Type { get; set; }
}