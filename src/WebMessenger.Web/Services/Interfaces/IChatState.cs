using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Web.Models;

namespace WebMessenger.Web.Services.Interfaces;

public interface IChatState
{
  public event Action<ChatModel?>? OnChatSelected;
  public event Action<ChatModel>? OnChatExit;
  public event Action? OnChangeChats;
  
  public ChatModel? CurrentChat { get; set; }
  public List<ChatModel> Chats { get; }
  
  public void AddChat(ChatModel chat);
  public void AddChats(IEnumerable<ChatModel> chats);

  public void ReceiveMessage(ChatMessageDto dto);
}