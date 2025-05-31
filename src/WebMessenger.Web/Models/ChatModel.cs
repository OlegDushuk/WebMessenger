using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Shared.Enums;

namespace WebMessenger.Web.Models;

public class ChatModel
{
  public ChatModel(ChatDto dto)
  {
    Id = dto.Id;
    Type = dto.Type;
    CurrentMember = new ChatMemberModel(dto.CurrentMember);
    
    if (dto.Type == ChatTypeDto.Personal)
    {
      AvatarUrl = dto.OtherMember!.Avatar;
      Name = dto.OtherMember!.Name;
    }
    else
    {
      AvatarUrl = dto.GroupDetails!.Avatar;
      Name = dto.GroupDetails!.Name;
      Bio = dto.GroupDetails!.Bio;
      GroupType = dto.GroupDetails!.Type;
    }
  }
  
  public Guid Id { get; set; }
  public ChatTypeDto Type { get; set; }
  public ChatMemberModel CurrentMember { get; set; }
  public string Name { get; set; }
  public string? AvatarUrl { get; set; }
  
  public string? Bio { get; set; }
  public GroupTypeDto? GroupType { get; set; }
  
  public string? CurrentMessage { get; set; }
  public bool FirstSelected { get; set; } = true;
  public int MessagePage { get; set; } = 1;
  public bool AllMessagesLoaded { get; set; }
  
  public List<MessageModel> Messages { get; set; } = [];
  public MessageModel? LastMessage => Messages.LastOrDefault();
  
  public List<ChatMemberModel> Members { get; set; } = [];
  public int NumberOfMembers => Members.Count;

  public Action? OnChangeState { get; set; }

  public void AddNewMessage(ChatMessageDto dto)
  {
    var sender = Members.First(m => m.Id == dto.MemberId);
    var model = new MessageModel(dto, sender, sender.UserId == CurrentMember.UserId);
    Messages.Add(model);
    
    OnChangeState?.Invoke();
  }
  
  public void AddHistory(IEnumerable<MessageModel> messages)
  {
    Messages.InsertRange(0, messages);
    
    OnChangeState?.Invoke();
  }

  public void AddMembers(IEnumerable<ChatMemberModel> members)
  {
    Members.AddRange(members);
    
    OnChangeState?.Invoke();
  }
}