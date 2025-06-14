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
  [Inject] private IChatApi ChatApi { get; set; } = null!;
  [Inject] private IChatState ChatState { get; set; } = null!;
  [Inject] private IChatNotificationService ChatNotificationService { get; set; } = null!;
  [Inject] private NavigationManager NavigationManager { get; set; } = null!;
  
  private ModalView _modalView = null!;
  
  private ModalTemplate _profile = null!;
  private ModalTemplate _createChatForm = null!;

  private bool _isAuthorized;
  private string? _searchQuery;
  private string _title = "Автентифікація";

  protected override async Task OnInitializedAsync()
  {
    _isAuthorized = await AuthState.IsAuthenticated();
    
    if (!_isAuthorized)
      await AuthenticateUserAsync();
    else
    {
      await FetchUserAsync();
      await FetchChatsAsync();
      _title = UserState.User?.Name ?? "";
    }
  }
  
  private async Task AuthenticateUserAsync()
  {
    await HttpHelper.FetchAsync(async () => await AuthApi.RefreshTokenAsync(),
      onSuccess: async response =>
      {
        var authResult = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(authResult))
          throw new NullReferenceException(nameof(authResult));

        await AuthState.Authenticate(authResult);
        _isAuthorized = true;
        StateHasChanged();
        
        await FetchUserAsync();
        await FetchChatsAsync();
        _title = UserState.User?.Name ?? "";
      },
      onFailure: async response =>
      {
        var error = await response.Content.ReadAsStringAsync();
        if (error == null)
          throw new NullReferenceException(nameof(error));

        NavigationManager.NavigateTo("/auth/login");
      },
      onException: _ =>
      {
        NavigationManager.NavigateTo("/auth/login");
      });
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
      onException: _ =>
      {
      });
  }
  
  private async Task FetchChatsAsync()
  {
    await HttpHelper.FetchAsync(async () => await ChatApi.GetChatsAsync(),
      onSuccess: async response =>
      {
        var dtos = await response.Content.ReadFromJsonAsync<List<ChatDto>>();
        if (dtos == null)
          throw new NullReferenceException(nameof(dtos));
        
        var chats = dtos.Select(dto => new ChatModel(dto)).ToList();
        ChatState.AddChats(chats);
        StateHasChanged();
        
        ChatNotificationService.OnReceiveMessage += dto =>
        {
          ChatState.ReceiveMessage(dto);
        };
        
        var chatIds = chats.Select(c => c.Id);
        await ChatNotificationService.InitConnectionAsync(UserState.User!.Id, chatIds);
      },
      onFailure: async response =>
      {
        var error = await response.Content.ReadAsStringAsync();
        if (error == null)
          throw new NullReferenceException(nameof(error));
      });
  }
}