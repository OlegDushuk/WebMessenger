using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace WebMessenger.Infrastructure.ClientConnection;

public class ClientHub(
  ConnectionClientStorage storage,
  ILogger<ClientHub> logger
  ) : Hub
{
  public async Task Init(Guid userId)
  {
    storage.AddUser(userId, Context.ConnectionId);
    
    logger.LogInformation("Connect user by id: {Id}", userId.ToString());
  }
  
  public async Task ConnectToChat(Guid chatId)
  {
    await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");
    
    logger.LogInformation("Client with connectionId: {Id} connect to chat: {Id}",
      Context.ConnectionId, chatId.ToString());
  }
  
  public async Task ConnectToChats(List<Guid> chatIds)
  {
    foreach (var chatId in chatIds)
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");
      
      logger.LogInformation("Client with connectionId: {Id} connect to chats",
        Context.ConnectionId);
    }
  }

  public async Task DisconnectChat(Guid chatId)
  {
    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{chatId}");
    
    logger.LogInformation("Client with connectionId: {Id} disconnect with chat: {Id}",
      Context.ConnectionId, chatId);
  }
  
  public override Task OnDisconnectedAsync(Exception? exception)
  {
    storage.RemoveUser(Context.ConnectionId);
    
    logger.LogInformation("Client with connectionId: {Id} disconnect with exception: {Exception}",
      Context.ConnectionId, exception?.Message);
    
    return base.OnDisconnectedAsync(exception);
  }
}