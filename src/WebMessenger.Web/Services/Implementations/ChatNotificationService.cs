using WebMessenger.Web.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Web.Services.Implementations;

public class ChatNotificationService : IChatNotificationService
{
  public event Action<ChatMessageDto>? OnReceiveMessage;
  public event Action<ChatDto>? OnReceivePrivateChat;
  
  private readonly HubConnection _connection;

  public ChatNotificationService()
  {
    _connection = new HubConnectionBuilder()
      .WithUrl("https://app-25061900251.azurewebsites.net/chat")
      .WithAutomaticReconnect()
      .Build();
    
    _connection.On<ChatMessageDto>("ReceiveMessage", message =>
    {
      OnReceiveMessage?.Invoke(message);
    });
    
    _connection.On<ChatDto>("ReceivePrivateChat", chat =>
    {
      OnReceivePrivateChat?.Invoke(chat);
    });
  }
  
  public async Task InitConnectionAsync(Guid userId)
  {
    await _connection.StartAsync();
    
    await _connection.InvokeAsync("Init", userId);
  }
  
  public async Task ConnectToChatAsync(Guid chatId)
  {
    await _connection.InvokeAsync("ConnectToChat", chatId);
  }

  public async Task ConnectToChatsAsync(List<Guid> chatIds)
  {
    await _connection.InvokeAsync("ConnectToChats", chatIds);
  }
}