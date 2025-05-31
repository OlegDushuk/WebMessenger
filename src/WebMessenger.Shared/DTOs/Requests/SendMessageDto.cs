namespace WebMessenger.Shared.DTOs.Requests;

public class SendMessageDto
{
  public Guid MemberId { get; set; }
  public string Content { get; set; } = string.Empty;
}