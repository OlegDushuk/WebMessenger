namespace WebMessenger.Application.DTOs.Responses;

public class AuthDto
{
  public string AccessToken { get; set; } = string.Empty;
  public string? RefreshToken { get; set; }
  public DateTime Expires { get; set; }
}