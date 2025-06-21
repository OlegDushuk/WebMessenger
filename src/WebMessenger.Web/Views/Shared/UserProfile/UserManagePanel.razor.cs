using Microsoft.AspNetCore.Components;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;

namespace WebMessenger.Web.Views.Shared.UserProfile;

public partial class UserManagePanel : ComponentBase
{
  [Inject] private IAuthApi AuthApi { get; set; } = null!;
  [Inject] private IAuthState AuthState { get; set; } = null!;
  [Inject] private NavigationManager NavManager { get; set; } = null!;

  private bool _isShowSettings;
  
  private void SwitchSettings()
  {
    _isShowSettings = !_isShowSettings;
  }
  
  private async Task HandleLogout()
  {
    await HttpHelper.FetchAsync(async () => await AuthApi.RevokeTokenAsync(),
      onSuccess: async _ =>
      {
        await AuthState.Logout();
        NavManager.NavigateTo("/auth/login", true);
      },
      onFailure: response =>
      {
        return Task.CompletedTask;
      },
      onException: exception =>
      {
        
      });
  }
}