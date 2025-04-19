using System.IdentityModel.Tokens.Jwt;
using WebMessenger.Web.Services.Interfaces;

namespace WebMessenger.Web.Services.Implementations;

public class AuthState : IAuthState
{
  public string? GetToken { get; private set; }
  
  public bool IsAuthenticated =>
    GetToken != null
    && _expiry > DateTime.UtcNow;
  
  public void Authenticate(string token)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    
    var tokenData = tokenHandler.ReadJwtToken(token);
    if (tokenData.Payload.TryGetValue("exp", out var expValue))
    {
      if (expValue is long expLong)
      {
        _expiry = DateTimeOffset.FromUnixTimeSeconds(expLong).DateTime.ToLocalTime();
      }
    }
    
    GetToken = token;
  }
  
  public void Logout()
  {
    GetToken = null;
  }
  
  private DateTime _expiry = DateTime.UtcNow;
}