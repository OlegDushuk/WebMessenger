using WebMessenger.Shared.Models;

namespace WebMessenger.Web.Services.Interfaces;

public interface IAuthApiSource
{
  Task<HttpResponseMessage> RegisterAsync(RegisterRequest request);
  
  Task<HttpResponseMessage> VerifyAsync(string token);
  
  Task<HttpResponseMessage> SendVerificationLinkAsync(string email);
  
  Task<HttpResponseMessage> SendResetPasswordLinkAsync(string email);
  
  Task<HttpResponseMessage> ResetPasswordAsync(ResetPasswordRequest request);
  
  Task<HttpResponseMessage> LoginAsync(LoginRequest login);
  
  Task<HttpResponseMessage> RefreshTokenAsync();
  
  Task<HttpResponseMessage> RevokeTokenAsync();
}