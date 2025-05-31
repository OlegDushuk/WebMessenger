using Microsoft.AspNetCore.SignalR;
using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Infrastructure.ClientConnection;

public class ClientHub(ConnectionClientStorage storage) : Hub
{
  public async Task Init(Guid userId, IEnumerable<Guid> chatIds)
  {
    storage.AddUser(userId, Context.ConnectionId);
    
    var enumerable = chatIds as Guid[] ?? chatIds.ToArray();
    foreach (var chatId in enumerable)
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");
    }
  }
  
  public override Task OnDisconnectedAsync(Exception? exception)
  {
    storage.RemoveUser(Context.ConnectionId);
    
    return base.OnDisconnectedAsync(exception);
  }
}