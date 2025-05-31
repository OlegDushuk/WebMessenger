using WebMessenger.Web.Models;

namespace WebMessenger.Web.Services.Interfaces;

public interface IChatState
{
  public event Action? OnChatSelected;
  public event Action? OnChatExit;
  public event Action? OnChangeChats;
  
  public ChatModel? CurrentChat { get; set; }
  public List<ChatModel> Chats { get; }
  
  public void AddChat(ChatModel chat);
  public void AddChats(IEnumerable<ChatModel> chats);
}