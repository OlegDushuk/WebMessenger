namespace WebMessenger.Web.Services.Interfaces;

public interface ISearchApi
{
  Task<HttpResponseMessage> SearchUsersAsync(string searchQuery);
  Task<HttpResponseMessage> SearchChatsAsync(string searchQuery);
}