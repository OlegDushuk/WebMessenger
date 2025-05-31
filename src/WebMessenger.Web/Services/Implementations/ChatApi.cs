using System.Net.Http.Json;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Web.Services.Interfaces;

namespace WebMessenger.Web.Services.Implementations;

public class ChatApi(
  HttpClient httpClient
) : IChatApi
{
  public async Task<HttpResponseMessage> GetChatsAsync()
  {
    return await httpClient.GetAsync("api/chat/chats");
  }

  public async Task<HttpResponseMessage> GetChatHistoryAsync(Guid chatId, int page, int pageSize)
  {
    return await httpClient.GetAsync($"api/chat/history/{chatId}/{page}/{pageSize}");
  }
  
  public async Task<HttpResponseMessage> GetChatMembers(Guid chatId)
  {
    return await httpClient.GetAsync($"api/chat/members/{chatId}");
  }

  public async Task<HttpResponseMessage> CreateGroupAsync(CreateGroupChatDto dto)
  {
    return await httpClient.PostAsJsonAsync("api/chat/group", dto);
  }
}