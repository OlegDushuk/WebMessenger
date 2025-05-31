namespace WebMessenger.Core.Entities;

public class ChatMessage
{
  public Guid Id { get; set; }
  public DateTime SendAt { get; set; }
  public Guid ChatId { get; set; }
  public Guid SenderId { get; set; }
  public Guid MemberId { get; set; }
  public string Content { get; set; } = string.Empty;
  public bool IsRead { get; set; }
  public bool IsEdited { get; set; }
  public int NumberOfLikes { get; set; }
}