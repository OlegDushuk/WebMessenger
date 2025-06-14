using WebMessenger.Web.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Web.Services.Implementations;

public class ChatNotificationService : IChatNotificationService
{
  public event Action<ChatMessageDto>? OnReceiveMessage;
  
  private readonly HubConnection _connection;

  public ChatNotificationService()
  {
    _connection = new HubConnectionBuilder()
      .WithUrl("https://localhost:7276/chat")
      .WithAutomaticReconnect()
      .Build();
    
    _connection.On<ChatMessageDto>("ReceiveMessage", message =>
    {
      OnReceiveMessage?.Invoke(message);
      Console.WriteLine(message.ChatId);
    });
  }
  
  public async Task InitConnectionAsync(Guid userId, IEnumerable<Guid> chatIds)
  {
    await _connection.StartAsync();
    
    await _connection.InvokeAsync("Init", userId, chatIds);
  }
  
  public async Task ConnectToChatAsync(Guid chatId)
  {
    await _connection.InvokeAsync("InitChat", chatId);
  }
}