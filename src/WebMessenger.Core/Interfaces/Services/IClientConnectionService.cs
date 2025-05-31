using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Core.Interfaces.Services;

public interface IClientConnectionService
{
  Task SendMessageToChat(Guid chatId, ChatMessageDto message);
}