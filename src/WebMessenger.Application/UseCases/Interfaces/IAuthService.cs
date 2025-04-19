using WebMessenger.Application.Common.Models;
using WebMessenger.Application.DTOs.Requests;
using WebMessenger.Application.DTOs.Responses;

namespace WebMessenger.Application.UseCases.Interfaces;

public interface IAuthService
{
  Task<Result> RegisterAsync(RegisterDto dto);
  Task<Result> VerifyEmailAsync(string token);
  Task<Result> ResendVerifyAsync(string email);
  Task<Result<AuthDto>> LoginAsync(LoginDto dto);
  Task<Result<string>> RefreshTokenAsync(string token);
  Task<Result> RevokeTokenAsync(string token);
  Task<Result> ForgotPasswordAsync(string email);
  Task<Result> ResetPasswordAsync(ResetPasswordDto dto);
}