namespace WebMessenger.Application.Common.Helpers.Settings;


public class JwtSettings
{
  public string Secret { get; set; } = null!;
  public int Expiration { get; set; }
}