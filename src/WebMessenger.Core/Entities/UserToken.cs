using WebMessenger.Core.Enums;

namespace WebMessenger.Core.Entities;

public class UserToken
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public string Token { get; set; } = string.Empty;
  public UserTokenType Type { get; set; }
  public DateTime ExpiresAt { get; set; }
}