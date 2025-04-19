using WebMessenger.Shared.Models;

namespace WebMessenger.Web.Services.Interfaces;

public interface IAccountApiSource
{
  Task<HttpResponseMessage> LoadUserAsync();
  
  Task<HttpResponseMessage> UpdateAccountDataAsync(UpdateAccountDataRequest request);
  
  Task<HttpResponseMessage> ChangePasswordAsync(ChangePasswordRequest request);
  
  Task<HttpResponseMessage> DeleteUserAsync();
}