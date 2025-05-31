using WebMessenger.Shared.Enums;

namespace WebMessenger.Shared.DTOs.Requests;

public class UpdateGroupChatDto
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? Bio { get; set; }
  public string? Avatar { get; set; }
  public GroupTypeDto? Type { get; set; }
}