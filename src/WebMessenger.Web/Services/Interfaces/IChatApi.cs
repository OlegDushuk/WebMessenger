using WebMessenger.Shared.DTOs.Requests;

namespace WebMessenger.Web.Services.Interfaces;

public interface IChatApi
{
  Task<HttpResponseMessage> GetChatsAsync();
  Task<HttpResponseMessage> GetChatHistoryAsync(Guid chatId, int page, int pageSize);
  Task<HttpResponseMessage> GetChatMembers(Guid chatId);
  Task<HttpResponseMessage> CreateGroupAsync(CreateGroupChatDto dto);
}