using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Web.Models;

public class MessageModel(ChatMessageDto dto, ChatMemberModel sender, bool isOwn)
{
  public Guid Id { get; } = dto.Id;
  public DateTime SendAt { get; } = dto.SendAt;
  public string Content { get; set; } = dto.Content;
  public bool IsRead { get; set; } = dto.IsRead;
  public bool IsEdited { get; set; } = dto.IsEdited;
  public int NumberOfLikes { get; set; } = dto.NumberOfLikes;
  
  public bool IsOwn { get; } = isOwn;
  public ChatMemberModel Sender { get; } = sender;
  
  public string FormatSendAt()
  {
    return TimeZoneInfo.ConvertTimeFromUtc(SendAt, TimeZoneInfo.Local)
      .ToString(SendAt.Date == DateTime.Today ? "HH:mm" : "dd.MM.yyyy");
  }
}