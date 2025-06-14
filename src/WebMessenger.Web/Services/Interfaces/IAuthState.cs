namespace WebMessenger.Web.Services.Interfaces;

public interface IAuthState
{
  Task<string?> GetToken();
  Task<bool> IsAuthenticated();
  Task Authenticate(string token);
  Task Logout();
}