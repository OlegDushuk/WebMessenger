using WebMessenger.Shared.Enums;

namespace WebMessenger.Shared.DTOs.Responses;

public class ChatDto
{
  public Guid Id { get; set; }
  public int NumberOfMessages { get; set; }
  public ChatTypeDto Type { get; set; }
  public ChatMemberDto CurrentMember { get; set; } = null!;
  
  public GroupDetailsDto? GroupDetails { get; set; }
  public ChatMemberDto? OtherMember { get; set; }
  
  public ChatMessageDto? LastMessage { get; set; }
  public ChatMemberDto? LastMessageSender { get; set; }
}