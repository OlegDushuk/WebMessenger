using Microsoft.AspNetCore.SignalR;
using WebMessenger.Core.Interfaces.Services;
using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Infrastructure.ClientConnection;

public class ClientConnectionService(IHubContext<ClientHub> hubContext) : IClientConnectionService
{
  public async Task SendMessageToChat(ChatMessageDto message)
  {
    await hubContext.Clients
      .Group($"{message.ChatId}")
      .SendAsync("ReceiveMessage", message);
  }
}