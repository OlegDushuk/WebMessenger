using Microsoft.AspNetCore.SignalR;
using WebMessenger.Core.Interfaces.Services;
using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Infrastructure.ClientConnection;

public class ClientConnectionService(
  IHubContext<ClientHub> hubContext,
  ConnectionClientStorage storage
  ) : IClientConnectionService
{
  public async Task NotifyUserCreatedPrivateChat(Guid userId, ChatDto chat)
  {
    var connectionId = storage.GetConnectionId(userId);
    if (connectionId == null)
      return;
    
    await hubContext.Clients
      .Client(connectionId)
      .SendAsync("ReceivePrivateChat", chat);
  }

  public async Task SendMessageToChat(ChatMessageDto message)
  {
    await hubContext.Clients
      .Group($"{message.ChatId}")
      .SendAsync("ReceiveMessage", message);
  }
}