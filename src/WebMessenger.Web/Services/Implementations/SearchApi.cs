using WebMessenger.Web.Services.Interfaces;

namespace WebMessenger.Web.Services.Implementations;

public class SearchApi(
  HttpClient httpClient
) : ISearchApi
{
  public async Task<HttpResponseMessage> SearchUsersAsync(string searchQuery)
  {
    return await httpClient.GetAsync($"api/search/user?searchRequest={searchQuery}");
  }

  public async Task<HttpResponseMessage> SearchChatsAsync(string searchQuery)
  {
    return await httpClient.GetAsync($"api/search/chat?searchRequest={searchQuery}");
  }
}