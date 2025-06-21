using WebMessenger.Application.Common.Models;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Shared.DTOs.Responses;

namespace WebMessenger.Application.UseCases.Interfaces;

public interface IAuthService
{
  Task<Result<RegisterResultDto>> RegisterAsync(RegisterDto dto);
  Task<Result> ConfirmEmailAsync(string token);
  Task<Result> ResendConfirmationAsync(string email);
  Task<Result<AuthDto>> LoginAsync(LoginDto dto);
  Task<Result<string>> RefreshAuthAsync(string token);
  Task<Result> RevokeAuthAsync(string token);
  Task<Result> ForgotPasswordAsync(string email);
  Task<Result> ResetPasswordAsync(ResetPasswordDto dto);
}