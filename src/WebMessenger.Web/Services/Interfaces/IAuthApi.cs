using WebMessenger.Shared.DTOs.Requests;

namespace WebMessenger.Web.Services.Interfaces;

public interface IAuthApi
{
  Task<HttpResponseMessage> RegisterAsync(RegisterDto request);
  
  Task<HttpResponseMessage> VerifyAsync(string token);
  
  Task<HttpResponseMessage> SendVerificationLinkAsync(string email);
  
  Task<HttpResponseMessage> SendResetPasswordLinkAsync(string email);
  
  Task<HttpResponseMessage> ResetPasswordAsync(ResetPasswordDto request);
  
  Task<HttpResponseMessage> LoginAsync(LoginDto login);
  
  Task<HttpResponseMessage> RefreshTokenAsync();
  
  Task<HttpResponseMessage> RevokeTokenAsync();
}