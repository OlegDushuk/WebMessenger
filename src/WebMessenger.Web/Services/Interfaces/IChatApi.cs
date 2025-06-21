using WebMessenger.Shared.DTOs.Requests;

namespace WebMessenger.Web.Services.Interfaces;

public interface IChatApi
{
  Task<HttpResponseMessage> GetChatsAsync();
  Task<HttpResponseMessage> GetChatHistoryAsync(Guid chatId, int startIndex, int pageSize);
  Task<HttpResponseMessage> GetChatMembers(Guid chatId);
  Task<HttpResponseMessage> CreateGroupAsync(CreateGroupChatDto dto);
  Task<HttpResponseMessage> CreatePrivateChatAsync(string otherUserName);
  Task<HttpResponseMessage> SendMessageAsync(SendMessageDto dto);
}