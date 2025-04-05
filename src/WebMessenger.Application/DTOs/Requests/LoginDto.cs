using MediatR;
using WebMessenger.Application.Common.Models;
using WebMessenger.Application.DTOs.Responses;

namespace WebMessenger.Application.DTOs.Requests;

public class LoginDto : IRequest<Result<AuthDto>>
{
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public bool RememberMe { get; set; }
}