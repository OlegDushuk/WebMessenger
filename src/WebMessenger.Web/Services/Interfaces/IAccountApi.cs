using WebMessenger.Shared.DTOs.Requests;

namespace WebMessenger.Web.Services.Interfaces;

public interface IAccountApi
{
  Task<HttpResponseMessage> LoadUserAsync();
  
  Task<HttpResponseMessage> UpdateAccountDataAsync(UpdateAccountDataDto request);
  
  Task<HttpResponseMessage> ChangePasswordAsync(ChangePasswordDto request);
  
  Task<HttpResponseMessage> DeleteUserAsync();
}