using WebMessenger.Core.Enums;

namespace WebMessenger.Core.Entities;

public class Chat
{
  public Guid Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public int NumberOfMessages { get; set; }
  public ChatType Type { get; set; }
  
  public GroupDetails? GroupDetails { get; set; }
}