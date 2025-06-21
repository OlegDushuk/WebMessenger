using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;

namespace WebMessenger.Web.Services.Implementations;

public class ChatState : IChatState
{
  private ChatModel? _currentChat;
  
  public event Action<ChatModel?>? OnChatSelected;
  public event Action<ChatModel>? OnChatExit;
  public event Action? OnChangeChats;
  
  public ChatModel? CurrentChat
  {
    get => _currentChat;
    set
    {
      if (_currentChat != null)
        OnChatExit?.Invoke(_currentChat);
      
      _currentChat = value;

      OnChatSelected?.Invoke(_currentChat);
    }
  }

  public List<ChatModel> Chats { get; set; } = [];
  
  public void AddChat(ChatModel chat)
  {
    Chats.Add(chat);
    
    OnChangeChats?.Invoke();
  }

  public void AddChats(IEnumerable<ChatModel> chats)
  {
    foreach (var chat in chats)
    {
      Chats.Add(chat);
    }
    
    OnChangeChats?.Invoke();
  }

  public void ReceiveMessage(ChatMessageDto dto)
  {
    var chat = Chats.First(x => x.Id == dto.ChatId);
    chat.AddNewMessage(dto);
  }
}