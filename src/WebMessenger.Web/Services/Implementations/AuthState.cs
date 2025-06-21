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
    if (token == null)
      return false;
    
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenData = tokenHandler.ReadJwtToken(token);
    var expiry = tokenData.ValidTo;
    
    return expiry > DateTime.UtcNow;
  }
  
  public async Task Authenticate(string token)
  {
    await storage.SetItemAsync("AccessToken", token);
  }
  
  public async Task Logout()
  {
    await storage.RemoveItemAsync("AccessToken");
  }
}