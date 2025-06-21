using WebMessenger.Shared.Enums;

namespace WebMessenger.Shared.DTOs.Responses;

public class ChatMemberDto
{
  public Guid Id { get; set; }
  public bool IsMuted { get; set; }
  public ChatMemberRoleDto Role { get; set; }
  
  public UserDto User { get; set; }
}
