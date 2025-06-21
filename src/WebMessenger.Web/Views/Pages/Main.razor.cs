using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using WebMessenger.Shared.DTOs.Requests;
using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Shared.Enums;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;
using WebMessenger.Web.Views.Shared.Modal;
using WebMessenger.Web.Views.Shared.Search;

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
  private SearchList _searchList = null!;
  
  private ModalTemplate _profile = null!;
  private ModalTemplate _createChatForm = null!;
  private ModalTemplate _chatInfo = null!;
  
  private bool _isAuthorized;
  private string? _searchQuery;
  private string _title = "Автентифікація";
  
  private CreateGroupModel _createGroupModel = new();
  private UserModel? _selectedUser;
  private ChatModel? _selectedChat;

  protected override async Task OnInitializedAsync()
  {
    _isAuthorized = await AuthState.IsAuthenticated();

    if (!_isAuthorized)
      await AuthenticateUserAsync();
    else
      await InitApp();
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
        
        await InitApp();
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

  private async Task InitApp()
  {
    ChatViewState.OnStateChanged += () => { InvokeAsync(StateHasChanged); };
    
    await FetchUserAsync();
    _title = UserState.User?.Name ?? "";
    await ChatNotificationService.InitConnectionAsync(UserState.User!.Id);
    await FetchChatsAsync();
    
    ChatNotificationService.OnReceiveMessage += dto =>
    {
      ChatState.ReceiveMessage(dto);
    };
    
    ChatNotificationService.OnReceivePrivateChat += dto =>
    {
      ChatState.AddChat(new ChatModel(dto));
      ChatNotificationService.ConnectToChatAsync(dto.Id);
    };
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
        
        var chatIds = chats.Select(c => c.Id).ToList();
        await ChatNotificationService.ConnectToChatsAsync(chatIds);
      },
      onFailure: async response =>
      {
        var error = await response.Content.ReadAsStringAsync();
        if (error == null)
          throw new NullReferenceException(nameof(error));
      });
  }
  
  private async Task HandleCreateChat()
  {
    await HttpHelper.FetchAsync(() => ChatApi.CreateGroupAsync(new CreateGroupChatDto
      {
        Name = _createGroupModel.Name!,
        Bio = _createGroupModel.Bio!,
        Avatar = _createGroupModel.AvatarUrl,
        Type = _createGroupModel.Type,
      }),
      onSuccess: async response =>
      {
        var chatDto = await response.Content.ReadFromJsonAsync<ChatDto>();
        if (chatDto == null)
          throw new NullReferenceException(nameof(chatDto));
        
        ChatState.AddChat(new ChatModel(chatDto));
        await ChatNotificationService.ConnectToChatAsync(chatDto.Id);
        
        _createGroupModel = new CreateGroupModel();
        _createChatForm.Close();
      },
      onFailure: async response =>
      {
        var errors = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        if (errors == null)
          throw new NullReferenceException(nameof(errors));
        
        
      },
      onException: exception =>
      {
        
      });
  }
  
  private async Task CreatePrivateChat(UserModel otherUser)
  {
    await HttpHelper.FetchAsync(() => ChatApi.CreatePrivateChatAsync(otherUser.UserName), 
      onSuccess: async response =>
      {
        var chatDto = await response.Content.ReadFromJsonAsync<ChatDto>();
        if (chatDto == null)
          throw new NullReferenceException(nameof(chatDto));

        var chat = new ChatModel(chatDto);
        
        ChatState.AddChat(chat);
        await ChatNotificationService.ConnectToChatAsync(chatDto.Id);
        
        _profile.Close();
        _searchQuery = null;
        
        ChatState.CurrentChat = chat;
        ChatViewState.IsShowing = true;
      },
      onFailure: async response =>
      {
        var errors = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        if (errors == null)
          throw new NullReferenceException(nameof(errors));
      });
  }
  
  private async Task HandleClickItem(SearchItemModel item)
  {
    _selectedUser = null;
    _selectedChat = null;
    
    if (item.Type == SearchItemTypeDto.Group)
    {
      _selectedChat = item.Chat;
      _chatInfo.Open();
    }
    else if (item.Type == SearchItemTypeDto.User)
    {
      _selectedUser = item.User;
      _profile.Open();
    }
  }
  
  private async Task HandleSearchQueryChange(ChangeEventArgs e)
  {
    var newValue = e.Value?.ToString();
    _searchQuery = newValue;
    await _searchList.Search(_searchQuery!);
  }

  private async Task OpenUserProfile()
  {
    _selectedUser = UserState.User;
    _profile.Open();
  }
}