using WebMessenger.Shared.Enums;

namespace WebMessenger.Shared.DTOs.Responses;

public class ChatMemberDto
{
  public Guid Id { get; set; }
  public bool IsMuted { get; set; }
  public ChatMemberRoleDto Role { get; set; }
  
  public Guid UserId { get; set; }
  public string UserName { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string? Avatar { get; set; }
}
