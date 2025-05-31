using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;
using WebMessenger.Web.Views.Shared.Modal;

namespace WebMessenger.Web.Views.Pages;

public partial class Main
{
  [Inject] private IAuthApi AuthApi { get; set; } = null!;
  [Inject] private IAccountApi AccountApi { get; set; } = null!;
  [Inject] private NavigationManager NavigationManager { get; set; } = null!;

  private ModalView _modalView = null!;
  
  private ModalTemplate _profile = null!;
  private ModalTemplate _createChatForm = null!;

  protected override async Task OnInitializedAsync()
  {
    if (!AuthState.IsAuthenticated)
      await AuthenticateUserAsync();
    else
      await FetchUserAsync();
  }
  
  private async Task AuthenticateUserAsync()
  {
    await HttpHelper.FetchAsync(async () => await AuthApi.RefreshTokenAsync(),
      onSuccess: async response =>
      {
        var authResult = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(authResult))
          throw new NullReferenceException(nameof(authResult));

        AuthState.Authenticate(authResult);
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
    await HttpHelper.FetchAsync(async () => await AccountApi.LoadUserAsync(),
      onSuccess: async response =>
      {
        var userResult = await response.Content.ReadFromJsonAsync<UserDto>();
        if (userResult == null)
          throw new NullReferenceException(nameof(userResult));

        UserState.User = new UserModel(userResult);
        StateHasChanged();
      },
      onFailure: async response =>
      {
        var error = await response.Content.ReadAsStringAsync();
        if (error == null)
          throw new NullReferenceException(nameof(error));
      },
      onException: ex =>
      {
      });
  }
}