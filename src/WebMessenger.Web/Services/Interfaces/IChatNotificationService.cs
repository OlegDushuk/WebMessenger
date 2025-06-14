using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Web.Services.Interfaces;

public interface IChatNotificationService
{
  event Action<ChatMessageDto>? OnReceiveMessage;
  
  Task InitConnectionAsync(Guid userId, IEnumerable<Guid> chatIds);
  Task ConnectToChatAsync(Guid chatId);
}