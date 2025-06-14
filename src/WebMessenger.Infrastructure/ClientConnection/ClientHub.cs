using Microsoft.AspNetCore.SignalR;
using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Infrastructure.ClientConnection;

public class ClientHub(ConnectionClientStorage storage) : Hub
{
  public async Task Init(Guid userId, List<Guid> chatIds)
  {
    storage.AddUser(userId, Context.ConnectionId);
    
    foreach (var chatId in chatIds)
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");
    }
  }
  
  public async Task InitChat(Guid chatId)
  {
    await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");
  }

  public async Task DisconnectChat(Guid chatId)
  {
    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{chatId}");
  }
  
  public override Task OnDisconnectedAsync(Exception? exception)
  {
    storage.RemoveUser(Context.ConnectionId);
    
    return base.OnDisconnectedAsync(exception);
  }
}