using WebMessenger.Core.Enums;

namespace WebMessenger.Core.Entities;

public class ChatMember
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public Guid ChatId { get; set; }
  public ChatMemberRole Role { get; set; }
  public bool IsMuted { get; set; }
  
  public User User { get; set; } = null!;
  public Chat Chat { get; set; } = null!;
}