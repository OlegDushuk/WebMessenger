using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using WebMessenger.Shared.Models;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;
using WebMessenger.Web.Views.Shared;

namespace WebMessenger.Web.Views.Pages;

public partial class Main
{
  [Inject] private IAuthApiSource AuthApiSource { get; set; }
  [Inject] private IAccountApiSource AccountApiSource { get; set; }
  [Inject] private NavigationManager NavigationManager { get; set; }

  private readonly Modal.ModalController _profileModalController = new();

  protected override async Task OnInitializedAsync()
  {
    if (!AuthState.IsAuthenticated)
    {
      await AuthenticateUserAsync();
    }
    else
    {
      if (UserState.User == null)
      {
        await FetchUserAsync();
      }
    }
  }
  
  private async Task AuthenticateUserAsync()
  {
    await HttpHelper.FetchAsync(async () => await AuthApiSource.RefreshTokenAsync(),
      onSuccess: async response =>
      {
        var authResult = await response.Content.ReadFromJsonAsync<AuthResult>();
        if (authResult == null)
          throw new NullReferenceException(nameof(authResult));

        AuthState.Authenticate(authResult.Token);
        
        await FetchUserAsync();
      },
      onFailure: async response =>
      {
        var error = await response.Content.ReadAsStringAsync();
        if (error == null)
          throw new NullReferenceException(nameof(error));

        NavigationManager.NavigateTo("/auth/login");
      },
      onException: exception => { NavigationManager.NavigateTo("/auth/login"); });
  }
  
  private async Task FetchUserAsync()
  {
    await HttpHelper.FetchAsync(async () => await AccountApiSource.LoadUserAsync(),
      onSuccess: async response =>
      {
        var userResult = await response.Content.ReadFromJsonAsync<UserResult>();
        if (userResult == null)
          throw new NullReferenceException(nameof(userResult));

        UserState.User = new User
        {
          Name = userResult.Name,
          UserName = userResult.UserName,
          Email = userResult.Email,
          AvatarUrl = userResult.Avatar,
          Bio = userResult.Bio,
        };
        
        StateHasChanged();
      },
      onFailure: async response =>
      {
        var error = await response.Content.ReadAsStringAsync();
        if (error == null)
          throw new NullReferenceException(nameof(error));
      });
  }
  
  private void OpenProfile()
  {
    _profileModalController.IsShow = true;
  }
}