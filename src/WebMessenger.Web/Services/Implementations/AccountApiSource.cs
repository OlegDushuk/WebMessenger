using System.Net.Http.Json;
using WebMessenger.Shared.Models;
using WebMessenger.Web.Services.Interfaces;

namespace WebMessenger.Web.Services.Implementations;

public class AccountApiSource(
  HttpClient httpClient
) : IAccountApiSource
{
  public async Task<HttpResponseMessage> LoadUserAsync()
  {
    return await httpClient.GetAsync("api/account/user");
  }

  public async Task<HttpResponseMessage> UpdateAccountDataAsync(UpdateAccountDataRequest request)
  {
    return await httpClient.PutAsJsonAsync("api/account/update-data", request);
  }

  public async Task<HttpResponseMessage> ChangePasswordAsync(ChangePasswordRequest request)
  {
    return await httpClient.PutAsJsonAsync("api/account/change-password", request);
  }

  public async Task<HttpResponseMessage> DeleteUserAsync()
  {
    return await httpClient.DeleteAsync("api/account/delete");
  }
}