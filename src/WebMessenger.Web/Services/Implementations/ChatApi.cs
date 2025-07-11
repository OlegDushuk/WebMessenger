﻿using System.Net.Http.Json;
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

  public async Task<HttpResponseMessage> GetChatHistoryAsync(Guid chatId, int startIndex, int pageSize)
  {
    return await httpClient.GetAsync($"api/chat/history/{chatId}/{startIndex}/{pageSize}");
  }
  
  public async Task<HttpResponseMessage> GetChatMembers(Guid chatId)
  {
    return await httpClient.GetAsync($"api/chat/members/{chatId}");
  }

  public async Task<HttpResponseMessage> CreateGroupAsync(CreateGroupChatDto dto)
  {
    return await httpClient.PostAsJsonAsync("api/chat/group", dto);
  }

  public async Task<HttpResponseMessage> CreatePrivateChatAsync(string otherUserName)
  {
    return await httpClient.PostAsync($"api/chat/private?otherUserName={otherUserName}", null);
  }

  public async Task<HttpResponseMessage> SendMessageAsync(SendMessageDto dto)
  {
    return await httpClient.PostAsJsonAsync("api/chat/send", dto);
  }
}