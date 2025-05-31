using WebMessenger.Web.Services.Interfaces;
using System.Net.Http.Json;
using WebMessenger.Shared.DTOs.Requests;

namespace WebMessenger.Web.Services.Implementations;

public class AuthApi(
  HttpClient httpClient
  ) : IAuthApi
{
  public async Task<HttpResponseMessage> RegisterAsync(RegisterDto request)
  {
    return await httpClient.PostAsJsonAsync("api/auth/reg", request);
  }
  
  public async Task<HttpResponseMessage> VerifyAsync(string token)
  {
    return await httpClient.PostAsync($"api/auth/verify?token={token}", null);
  }

  public async Task<HttpResponseMessage> SendVerificationLinkAsync(string email)
  {
    return await httpClient.PostAsync($"api/auth/resend-verify?email={email}", null);
  }
  
  public async Task<HttpResponseMessage> SendResetPasswordLinkAsync(string email)
  {
    return await httpClient.PostAsync($"api/auth/forgot-password?email={email}", null);
  }

  public async Task<HttpResponseMessage> ResetPasswordAsync(ResetPasswordDto request)
  {
    return await httpClient.PostAsJsonAsync("api/auth/reset-password", request);
  }

  public async Task<HttpResponseMessage> LoginAsync(LoginDto request)
  {
    return await httpClient.PostAsJsonAsync("api/auth/login", request);
  }

  public async Task<HttpResponseMessage> RefreshTokenAsync()
  {
    return await httpClient.GetAsync("api/auth/refresh-token");
  }

  public async Task<HttpResponseMessage> RevokeTokenAsync()
  {
    return await httpClient.PostAsync("api/auth/revoke-token", null);
  }
}