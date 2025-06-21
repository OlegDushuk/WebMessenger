using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;
using WebMessenger.Web.Views.Shared.Modal;

namespace WebMessenger.Web.Views.Shared.Search;

public partial class SearchList : ComponentBase
{
  [Parameter] public EventCallback<SearchItemModel> OnClickItem { get; set; }

  [Inject] private ISearchApi SearchApi { get; set; } = null!;
  [Inject] private IChatApi ChatApi { get; set; } = null!;
  [Inject] private IChatState ChatState { get; set; } = null!;
  [Inject] private IChatNotificationService ChatNotificationService { get; set; } = null!;
  [Inject] private ChatViewState ChatViewState { get; set; } = null!;
  
  private readonly List<SearchItemModel> _items = [];
  private string _query = string.Empty;
  
  private bool _isLoading;
  private bool _notFound;
  private string _prevRequire = string.Empty;
  
  private ModalTemplate _userInfo = null!;
  private ModalTemplate _chatInfo = null!;
  private UserModel? _selectedUser;
  private ChatModel? _selectedChat;
  
  public async Task Search(string query)
  {
    if (query == string.Empty)
      return;
    
    _query = query;
    _isLoading = true;
    _notFound = false;
    _items.Clear();
    
    Func<Task<HttpResponseMessage>> request;
    if (query[0] == '@')
      request = () => SearchApi.SearchUsersAsync(query[1..]);
    else
      request = () => SearchApi.SearchChatsAsync(query);

    await HttpHelper.FetchAsync(request.Invoke,
      onSuccess: async response =>
      {
        var result = await response.Content.ReadFromJsonAsync<List<SearchItemDto>>();
        if (result == null)
          throw new NullReferenceException(nameof(result));

        _items.AddRange(result.Select(dto => new SearchItemModel(dto)));
        Console.WriteLine(_items.Count);
      },
      onFailure: async response =>
      {
        var error = await response.Content.ReadAsStringAsync();
        if (error == null)
          throw new NullReferenceException(nameof(error));

        _notFound = true;
      });
    
    _isLoading = false;
    _prevRequire = query;
  }

  private async Task HandleClickItem(SearchItemModel item)
  {
    if (OnClickItem.HasDelegate)
      await OnClickItem.InvokeAsync(item);
  }
}