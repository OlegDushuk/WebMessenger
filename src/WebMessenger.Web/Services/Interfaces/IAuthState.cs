namespace WebMessenger.Web.Services.Interfaces;

public interface IAuthState
{
  string? GetToken { get; }
  bool IsAuthenticated { get; }
  
  void Authenticate(string token);
  void Logout();
}