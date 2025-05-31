namespace WebMessenger.Shared.DTOs.Responses;

public class AuthDto
{
  public string AccessToken { get; set; } = string.Empty;
  public string? RefreshToken { get; set; }
}