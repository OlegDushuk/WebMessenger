using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Web.Services.Interfaces;

public interface IChatNotificationService
{
  event Action<ChatMessageDto>? OnReceiveMessage;
  event Action<ChatDto>? OnReceivePrivateChat;
  
  Task InitConnectionAsync(Guid userId);
  Task ConnectToChatAsync(Guid chatId);
  Task ConnectToChatsAsync(List<Guid> chatIds);
}