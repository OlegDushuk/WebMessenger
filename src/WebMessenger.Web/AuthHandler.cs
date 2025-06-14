using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using WebMessenger.Web.Services.Interfaces;

namespace WebMessenger.Web;

public class AuthHandler : DelegatingHandler
{
  private readonly IAuthState _authState;
  
  public AuthHandler(IAuthState authState)
  {
    _authState = authState;
    InnerHandler = new HttpClientHandler();
  }
  
  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
    
    var token = await _authState.GetToken();
    if (!string.IsNullOrEmpty(token))
      request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
    return await base.SendAsync(request, cancellationToken);
  }
}