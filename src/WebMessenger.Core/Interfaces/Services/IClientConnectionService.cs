using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Core.Interfaces.Services;

public interface IClientConnectionService
{
  Task NotifyUserCreatedPrivateChat(Guid userId, ChatDto chat);
  
  Task SendMessageToChat(ChatMessageDto message);
}