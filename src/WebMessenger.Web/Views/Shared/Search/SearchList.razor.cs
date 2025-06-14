using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Web.Models;
using WebMessenger.Web.Services.Interfaces;
using WebMessenger.Web.Utils;

namespace WebMessenger.Web.Views.Shared.Search;

public partial class SearchList : ComponentBase
{
  [Parameter] public string SearchRequest { get; set; } = null!;

  [Inject] private ISearchApi SearchApi { get; set; } = null!;
  
  private readonly List<SearchItemModel> _items = [];
  
  private bool _isLoading;
  private bool _notFound;
  
  protected override async Task OnParametersSetAsync()
  {
    _isLoading = true;
    _notFound = false;
    _items.Clear();
    
    Func<Task<HttpResponseMessage>> request;
    if (SearchRequest[0] == '@')
      request = () => SearchApi.SearchUsersAsync(SearchRequest[1..]);
    else
      request = () => SearchApi.SearchChatsAsync(SearchRequest);

    await HttpHelper.FetchAsync(request.Invoke,
      onSuccess: async response =>
      {
        var result = await response.Content.ReadFromJsonAsync<List<SearchItemDto>>();
        if (result == null)
          throw new NullReferenceException(nameof(result));

        _items.AddRange(result.Select(dto => new SearchItemModel(dto)));
      },
      onFailure: async response =>
      {
        var error = await response.Content.ReadAsStringAsync();
        if (error == null)
          throw new NullReferenceException(nameof(error));

        _notFound = true;
      });
    
    _isLoading = false;
    
    await base.OnParametersSetAsync();
  }
}