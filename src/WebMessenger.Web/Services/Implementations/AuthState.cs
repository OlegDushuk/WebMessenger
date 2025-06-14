using System.IdentityModel.Tokens.Jwt;
using Blazored.SessionStorage;
using WebMessenger.Web.Services.Interfaces;

namespace WebMessenger.Web.Services.Implementations;

public class AuthState(ISessionStorageService storage) : IAuthState
{
  public async Task<string?> GetToken()
  {
    return await storage.GetItemAsync<string>("AccessToken");
  }
  
  public async Task<bool> IsAuthenticated()
  {
    var token = await GetToken();
    return token != null && _expiry > DateTime.UtcNow;
  }
  
  public async Task Authenticate(string token)
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

    await storage.SetItemAsync("AccessToken", token);
  }
  
  public async Task Logout()
  {
    await storage.RemoveItemAsync("AccessToken");
  }
  
  private DateTime _expiry = DateTime.UtcNow;
}