namespace WebMessenger.Core.Entities;

public class UnreadMessage
{
  public Guid MemberId { get; set; }
  public Guid MessageId { get; set; }
}