using System.Net.Http.Json;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Web.Services.Interfaces;

namespace WebMessenger.Web.Services.Implementations;

public class AccountApi(
  HttpClient httpClient
) : IAccountApi
{
  public async Task<HttpResponseMessage> LoadUserAsync()
  {
    return await httpClient.GetAsync("api/account/user");
  }

  public async Task<HttpResponseMessage> UpdateAccountDataAsync(UpdateAccountDataDto request)
  {
    return await httpClient.PutAsJsonAsync("api/account/update-data", request);
  }

  public async Task<HttpResponseMessage> ChangePasswordAsync(ChangePasswordDto request)
  {
    return await httpClient.PutAsJsonAsync("api/account/change-password", request);
  }

  public async Task<HttpResponseMessage> DeleteUserAsync()
  {
    return await httpClient.DeleteAsync("api/account/delete");
  }
}